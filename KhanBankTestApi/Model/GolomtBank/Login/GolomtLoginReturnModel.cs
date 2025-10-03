namespace KhanBankTestApi.Model.GolomtBank.Login
{
    public class GolomtLoginReturnModel
    {
        public string? requestId { get; set; }
        public string? token { get; set; }
        public string? refreshToken { get; set; }
        public string? tokenType { get; set; }
        public decimal expiresIn { get; set; }
        public string? Error { get; set; }
        public string? EncryptedKey { get; set; }
    }
}
