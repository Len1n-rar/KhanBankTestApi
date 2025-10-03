namespace KhanBankTestApi.Model.GolomtBank.GetToken
{
    public class GolomtAccountReturnModel
    {
        public string? requestId { get; set; }
        public List<GolomtAccountReturnLineModel>? operAccounts { get; set; }
        public List<GolomtAccountReturnLineModel>? depoAccounts { get; set; }
        public List<GolomtAccountReturnLineModel>? loanAccounts { get; set; }
        public string? Error { get; set; }
    }
}
