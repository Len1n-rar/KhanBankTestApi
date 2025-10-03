namespace KhanBankTestApi.Model.GolomtBank.Login
{
    public class GolomtLoginModel
    {
        public string? Url { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Service { get; set; }
        public string? CheckSum { get; set; }
        public string? SessionKey { get; set; }
        public string? IvKey { get; set; }
    }
}
