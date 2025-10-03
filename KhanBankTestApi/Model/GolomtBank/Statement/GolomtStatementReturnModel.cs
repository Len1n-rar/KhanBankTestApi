namespace KhanBankTestApi.Model.GolomtBank.Statement
{
    public class GolomtStatementReturnModel
    {
        public string? requestId { get; set; }
        public string? accountId { get; set; }
        public List<GolomtStatementReturnLineModel>? statements { get; set; }
        public string? Error { get; set; }
    }
}
