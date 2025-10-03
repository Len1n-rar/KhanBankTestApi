using System.Text;
using System.Text.Json;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using KhanBankTestApi.Model.KhanBank.Login;
using KhanBankTestApi.Model.KhanBank.Account;
using KhanBankTestApi.Model.KhanBank.Domestic;
using KhanBankTestApi.Model.KhanBank.InterBank;
using KhanBankTestApi.Model.KhanBank.Statement;

namespace KhanBankTestApi.KhanBank
{
    [ApiController]
    [Route("[controller]")]
    public class KhanBankController : Controller
    {

        private readonly ILogger<KhanBankController> _logger;

        public KhanBankController(ILogger<KhanBankController> logger)
        {
            _logger = logger;
        }

        #region Get Token
        [HttpPost("gettoken")]
        public async Task<LoginReturnModel> GetToken([FromBody] LoginModel login)
        {
            try
            {
                string authValue = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{login.Username}:{login.Password}"));

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authValue);

                    var requestBody = new StringContent("grant_type=client_credentials", Encoding.UTF8);
                    requestBody.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

                    HttpResponseMessage response = await client.PostAsync(login.Url, requestBody);
                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        var data = JsonSerializer.Deserialize<LoginReturnModel>(result);
                        if (data != null)
                            return data;
                    }

                    return new LoginReturnModel() { error = "error" };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                var ReturnModel = new LoginReturnModel();
                ReturnModel.error = ex.Message;
                return ReturnModel;
            }
        }
        #endregion

        #region Get Account

        [HttpGet("getaccount")]
        public async Task<AccountsResponse> GetAccount([FromBody] AccountModel login)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", login.Token);
                    HttpResponseMessage response = await client.GetAsync(login.Url);

                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                        var data = JsonSerializer.Deserialize<AccountsResponse>(json, options);
                        if (data?.accounts != null)
                            return data;
                    }
                    return new AccountsResponse() { error = "error" };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                var ReturnModel = new AccountsResponse();
                ReturnModel.error = ex.Message;
                return ReturnModel;
            }
        }
        #endregion

        #region Get Statement
        [HttpGet("getstatement")]
        public async Task<StatementResponse> GetStatement([FromBody] StatementModel login)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", login.Token);
                    HttpResponseMessage response = await client.GetAsync(login.Url);

                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                        var data = JsonSerializer.Deserialize<StatementResponse>(json, options);
                        if (data != null)
                            return data;
                    }
                    return new StatementResponse() { error = "error" };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                var ReturnModel = new StatementResponse();
                ReturnModel.error = ex.Message;
                return ReturnModel;
            }
        }

        #endregion

        #region Transfer domestic
        [HttpPost("transferdomestic")]
        public async Task<DomesticResponse> TransferDomestic([FromBody] TransferDomesticModel model)
        {
            try
            {
                var domesticModel = new DomesticModel
                {
                    fromAccount = model.fromAccount,
                    toAccount = model.toAccount,
                    toCurrency = model.toCurrency,
                    amount = model.amount,
                    description = model.description,
                    currency = model.currency,
                    loginName = model.loginName,
                    tranPassword = model.tranPassword,
                    transferid = model.transferid,
                };

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", model.Token);


                    string jsonBody = JsonSerializer.Serialize(domesticModel);


                    var requestBody = new StringContent(jsonBody, Encoding.UTF8);
                    requestBody.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    HttpResponseMessage response = await client.PostAsync(model.Url, requestBody);

                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                        var data = JsonSerializer.Deserialize<DomesticResponse>(json, options);
                        if (data != null)
                            return data;
                    }
                    return new DomesticResponse() { error = "error" };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                var ReturnModel = new DomesticResponse();
                ReturnModel.error = ex.Message;
                return ReturnModel;
            }
        }
        #endregion

        #region Transfer Interbank
        [HttpPost("transferinterbank")]
        public async Task<InterBankResponse> TransferInterBank([FromBody] TransferInterBankModel model)
        {
            try
            {
                var interBankModel = new InterBankModel
                {
                    fromAccount = model.fromAccount,
                    toAccount = model.toAccount,
                    toCurrency = model.toCurrency,
                    amount = model.amount,
                    description = model.description,
                    currency = model.currency,
                    loginName = model.loginName,
                    tranPassword = model.tranPassword,
                    transferid = model.transferid,
                };

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", model.Token);


                    string jsonBody = JsonSerializer.Serialize(interBankModel);


                    var requestBody = new StringContent(jsonBody, Encoding.UTF8);
                    requestBody.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    HttpResponseMessage response = await client.PostAsync(model.Url, requestBody);

                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                        var data = JsonSerializer.Deserialize<InterBankResponse>(json, options);
                        if (data != null)
                            return data;
                    }
                    return new InterBankResponse() { error = "error" };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                var ReturnModel = new InterBankResponse();
                ReturnModel.error = ex.Message;
                return ReturnModel;
            }
        }
        #endregion
    }
}
