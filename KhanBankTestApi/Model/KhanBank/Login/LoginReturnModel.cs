namespace KhanBankTestApi.Model.KhanBank.Login
{
    public class LoginReturnModel
    {
        public string? access_token { get; set; }
        public int access_token_expires_in { get; set; }
        public string? organization_name { get; set; }
        public string? developer_email { get; set; }
        public string? token_type { get; set; }
        public string? error { get; set; }

    }
}
