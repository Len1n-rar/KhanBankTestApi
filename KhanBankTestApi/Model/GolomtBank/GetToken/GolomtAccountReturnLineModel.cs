namespace KhanBankTestApi.Model.GolomtBank.GetToken
{
    public class GolomtAccountReturnLineModel
    {
        public string? requestId { get; set; }
        public string? accountId { get; set; }
        public string? accountName { get; set; }
        public string? shortName { get; set; }
        public string? currency { get; set; }
        public string? branchId { get; set; }
        public string? cifId { get; set; }
        public string? accountNumber { get; set; }
        public string? isSocialPayConnected { get; set; }
        public GolomtAccountType? accountType { get; set; }
    }
}
