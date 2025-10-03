using KhanBankTestApi.Model.GolomtBank.Login;
using KhanBankTestApi.Model.GolomtBank.Balance;
using KhanBankTestApi.Model.GolomtBank.GetToken;
using KhanBankTestApi.Model.GolomtBank.Statement;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.Routing;

namespace KhanBankTestApi.GolomtBank
{
    [ApiController]
    [Route("[controller]")]
    public class GolomtBankController : Controller
    {

        private readonly ILogger<GolomtBankController> _logger;

        public GolomtBankController(ILogger<GolomtBankController> logger)
        {
            _logger = logger;
        }

        #region Log in 
        [HttpPost("login")]
        public async Task<GolomtLoginReturnModel> Login([FromBody] GolomtLoginModel model)
        {
            try
            {
                var encryptedKey = EncryptPassword(model.Password, model.SessionKey, model.IvKey);

                var sendModel = new GolomtLoginSendModel 
                { 
                    name = model.Username,
                    password = encryptedKey,
                };

                using (HttpClient client = new HttpClient())
                {

                    string jsonBody = JsonSerializer.Serialize(sendModel);

                    var requestBody = new StringContent(jsonBody, Encoding.UTF8);
                    requestBody.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    requestBody.Headers.Add("X-Golomt-Service", model.Service);
                    requestBody.Headers.Add("X-Golomt-Checksum", model.CheckSum);

                    HttpResponseMessage response = await client.PostAsync(model.Url, requestBody);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        var parse = JsonDocument.Parse(result);

                        var data = new GolomtLoginReturnModel() 
                        {
                            requestId = parse.RootElement.GetProperty("requestId").GetString()?? string.Empty,
                            token = parse.RootElement.GetProperty("token").GetString() ?? string.Empty,
                            refreshToken = parse.RootElement.GetProperty("refreshToken").GetString() ?? string.Empty,
                            tokenType = parse.RootElement.GetProperty("tokenType").GetString() ?? string.Empty,
                            expiresIn = parse.RootElement.GetProperty("expiresIn").GetDecimal(),
                            EncryptedKey = encryptedKey,
                            Error = string.Empty
                        };

                        if (data != null)
                            return data;
                    }
                    return new GolomtLoginReturnModel() { Error = "error" };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                var ReturnModel = new GolomtLoginReturnModel();
                ReturnModel.Error = ex.Message;
                return ReturnModel;
            }
        }

        #endregion

        #region Get Accounts
        [HttpPost("getaccount")]
        public async Task<GolomtAccountReturnModel> GetAccounts([FromBody] GolomtAccountModel model)
        {
            try
            {
                var sendModel = new GolomtAccountSendModel
                {
                    registerNo = model.RegisterNo,
                };

                var jsonBody = JsonSerializer.Serialize(sendModel);
                var checksum = EncryptKey(jsonBody, model.SessionKey, model.IvKey);

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", model.AccessToken);

                    var requestBody = new StringContent(jsonBody, Encoding.UTF8);
                    requestBody.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    requestBody.Headers.Add("X-Golomt-Service", model.Service);
                    requestBody.Headers.Add("X-Golomt-Checksum", checksum);

                    HttpResponseMessage response = await client.PostAsync(model.Url, requestBody);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        var decrypted = DecryptResult(result, model.SessionKey, model.IvKey);
                        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                        var data = JsonSerializer.Deserialize<GolomtAccountReturnModel>(decrypted, options);
                        if (data != null)
                            return data;
                    }

                    return new GolomtAccountReturnModel() { Error = "error" };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                var ReturnModel = new GolomtAccountReturnModel();
                ReturnModel.Error = ex.Message;
                return ReturnModel;
            }
        }
        #endregion

        #region Get Balance
        [HttpPost("getbalance")]
        public async Task<GolomtBalanceReturnModel> GetAccounts([FromBody] GolomtBalanceModel model)
        {
            try
            {
                var sendModel = new GolomtBalanceSendModel
                {
                    registerNo = model.RegisterNo,
                    accountId = model.AccountId,
                };

                var jsonBody = JsonSerializer.Serialize(sendModel);
                var checksum = EncryptKey(jsonBody, model.SessionKey, model.IvKey);

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", model.AccessToken);

                    var requestBody = new StringContent(jsonBody, Encoding.UTF8);
                    requestBody.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    requestBody.Headers.Add("X-Golomt-Service", model.Service);
                    requestBody.Headers.Add("X-Golomt-Checksum", checksum);

                    HttpResponseMessage response = await client.PostAsync(model.Url, requestBody);

                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        var decrypted = DecryptResult(result, model.SessionKey, model.IvKey);
                        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                        var data = JsonSerializer.Deserialize<GolomtBalanceReturnModel>(decrypted, options);
                        if (data != null)
                            return data;
                    }

                    return new GolomtBalanceReturnModel() { Error = "error" };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                var ReturnModel = new GolomtBalanceReturnModel();
                ReturnModel.Error = ex.Message;
                return ReturnModel;
            }
        }

        #endregion

        #region Get Statement
        [HttpPost("getstatement")]
        public async Task<GolomtStatementReturnModel> GetStatement([FromBody] GolomtStatementModel model)
        {
            try
            {
                var sendModel = new GolomtStatementSendModel
                {
                    registerNo = model.RegisterNo,
                    accountId = model.AccountId,
                    startDate = model.StartDate,
                    endDate = model.EndDate,
                };

                var jsonBody = JsonSerializer.Serialize(sendModel);
                var checksum = EncryptKey(jsonBody, model.SessionKey, model.IvKey);

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", model.AccessToken);

                    var requestBody = new StringContent(jsonBody, Encoding.UTF8);
                    requestBody.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    requestBody.Headers.Add("X-Golomt-Service", model.Service);
                    requestBody.Headers.Add("X-Golomt-Checksum", checksum);

                    HttpResponseMessage response = await client.PostAsync(model.Url, requestBody);

                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        var decrypted = DecryptResult(result, model.SessionKey, model.IvKey);
                        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                        var data = JsonSerializer.Deserialize<GolomtStatementReturnModel>(decrypted, options);
                        if (data != null)
                            return data;
                    }

                    return new GolomtStatementReturnModel() { Error = "error" };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                var ReturnModel = new GolomtStatementReturnModel();
                ReturnModel.Error = ex.Message;
                return ReturnModel;
            }
        }
        #endregion

        #region Encrypt & Decrypt
        private string EncryptKey(string? plainText, string? SessionKey, string? IvKey)
        {
            byte[] key = Encoding.Latin1.GetBytes(SessionKey ?? string.Empty);
            byte[] iv = Encoding.Latin1.GetBytes(IvKey ?? string.Empty);
            byte[] hashBytes;
            string hexString;

            using (SHA256 sha256 = SHA256.Create())
            {
                var ss = Encoding.UTF8.GetBytes(plainText ?? string.Empty);
                hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(plainText ?? string.Empty));

                hexString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
            return Encrypt(hexString, key, iv);
        }

        private string EncryptPassword(string? plainText, string? SessionKey, string? IvKey)
        {
            byte[] key = Encoding.Latin1.GetBytes(SessionKey ?? string.Empty);
            byte[] iv = Encoding.Latin1.GetBytes(IvKey ?? string.Empty);

            return Encrypt(plainText, key, iv);
        }

        private string Encrypt(string? plainText, byte[] key, byte[] iv)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Padding = PaddingMode.PKCS7;

                var ss= BitConverter.ToString(key);

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                {
                    swEncrypt.Write(plainText);
                    swEncrypt.Close();
                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }

        private string DecryptResult(string? responseBody, string? SessionKey, string? IvKey)
        {
            byte[] cipherBytes = Convert.FromBase64String(responseBody?? string.Empty);
            byte[] keyBytes = Encoding.GetEncoding("ISO-8859-1").GetBytes(SessionKey ?? string.Empty);
            byte[] ivBytes = Encoding.GetEncoding("ISO-8859-1").GetBytes(IvKey ?? string.Empty);

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = keyBytes;
                aesAlg.IV = ivBytes;
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Padding = PaddingMode.PKCS7;

                using (ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV))
                using (MemoryStream msDecrypt = new MemoryStream(cipherBytes))
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                using (StreamReader srDecrypt = new StreamReader(csDecrypt, Encoding.UTF8))
                {
                    return srDecrypt.ReadToEnd();
                }
            }

        }

        #endregion

        #region Get Encrypt
        [HttpPost("getencrypt")]
        public async Task<string> GetEncrypt([FromBody] string Key)
        {
            try
            {
                var ss= GenerateCurrentNumberString(Key);
                return ss;
            }
            catch (Exception ex)
            {
                return "error";
            }
        }

        public const int DEFAULT_TIME_STEP_SECONDS = 30;
        private static int NUM_DIGITS_OUTPUT = 6;
        private static readonly string blockOfZeros;

        public static string GenerateCurrentNumberString(string base32Secret)
        {
            var  dateTime = new DateTime(2025, 9, 19, 11, 59, 0);
            var Ticks = dateTime.Ticks;
            var millSeconds = TimeSpan.TicksPerMillisecond;

            var tounixdate = (Ticks - 621355968000000000) / millSeconds;

            return GenerateNumberString(base32Secret, tounixdate, DEFAULT_TIME_STEP_SECONDS);

            return GenerateNumberString(base32Secret, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(), DEFAULT_TIME_STEP_SECONDS);
        }

        public static string GenerateNumberString(string base32Secret, long timeMillis, int timeStepSeconds)
        {
            int number = GenerateNumber(base32Secret, timeMillis, timeStepSeconds);
            return ZeroPrepend(number, NUM_DIGITS_OUTPUT);
        }
        public static int GenerateNumber(string base32Secret, long timeMillis, int timeStepSeconds)
        {
            byte[] key = DecodeBase32(base32Secret);
            byte[] data = new byte[8];
            long value = timeMillis / 1000L / timeStepSeconds;

            for (int i = 7; value > 0; i--)
            {
                data[i] = (byte)(value & 0xFF);
                value >>= 8;
            }

            using (var hmac = new HMACSHA1(key))
            {
                byte[] hash = hmac.ComputeHash(data);
                var ss = hash[hash.Length - 1];
                int offset = hash[hash.Length - 1] & 0x0F;

                long truncatedHash = 0;
                for (int i = offset; i < offset + 4; i++)
                {
                    truncatedHash <<= 8;
                    truncatedHash |= (hash[i] & 0xFF);
                }

                truncatedHash &= 0x7FFFFFFF;
                truncatedHash %= 1000000;

                return (int)truncatedHash;
            }
        }

        private static string ZeroPrepend(int num, int digits)
        {
            string numStr = num.ToString();
            if (numStr.Length >= digits)
                return numStr;
            var result1 = blockOfZeros.Substring(0, digits - numStr.Length);
            var result = result1 + numStr;

            return result;
        }

        private static byte[] DecodeBase32(string str)
        {
            int numBytes = (str.Length * 5 + 7) / 8;
            byte[] result = new byte[numBytes];

            int resultIndex = 0;
            int which = 0;
            int working = 0;

            foreach (char ch in str)
            {
                int val;
                if (ch >= 'a' && ch <= 'z')
                    val = ch - 'a';
                else if (ch >= 'A' && ch <= 'Z')
                    val = ch - 'A';
                else if (ch >= '2' && ch <= '7')
                    val = 26 + (ch - '2');
                else if (ch == '=')
                {
                    which = 0;
                    break;
                }
                else
                    throw new ArgumentException("Invalid base-32 character: " + ch);

                switch (which)
                {
                    case 0:
                        working = (val & 31) << 3;
                        which = 1;
                        break;
                    case 1:
                        working |= (val & 28) >> 2;
                        result[resultIndex++] = (byte)working;
                        working = (val & 3) << 6;
                        which = 2;
                        break;
                    case 2:
                        working |= (val & 31) << 1;
                        which = 3;
                        break;
                    case 3:
                        working |= (val & 16) >> 4;
                        result[resultIndex++] = (byte)working;
                        working = (val & 15) << 4;
                        which = 4;
                        break;
                    case 4:
                        working |= (val & 30) >> 1;
                        result[resultIndex++] = (byte)working;
                        working = (val & 1) << 7;
                        which = 5;
                        break;
                    case 5:
                        working |= (val & 31) << 2;
                        which = 6;
                        break;
                    case 6:
                        working |= (val & 24) >> 3;
                        result[resultIndex++] = (byte)working;
                        working = (val & 7) << 5;
                        which = 7;
                        break;
                    case 7:
                        working |= val & 31;
                        result[resultIndex++] = (byte)working;
                        which = 0;
                        break;
                }
            }

            if (which != 0)
                result[resultIndex++] = (byte)working;

            if (resultIndex != result.Length)
                Array.Resize(ref result, resultIndex);

            return result;
        }
        #endregion
    }
}
