namespace KhanBankTestApi.Model.GolomtBank.Balance
{
    public class GolomtBalanceReturnModel
    {
        public string? requestId { get; set; }
        public string? accountId { get; set; }
        public string? accountName { get; set; }
        public string? currency { get; set; }
        public List<GolomtBalanceReturnLineModel>? balanceLL { get; set; }
        public string? Error { get; set; }
    }
}
