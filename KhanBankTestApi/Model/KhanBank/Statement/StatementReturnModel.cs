namespace KhanBankTestApi.Model.KhanBank.Statement
{
    public class StatementReturnModel
    {
        public string? description { get; set; }
        public decimal record { get; set; }
        public decimal amount { get; set; }
        public DateTime tranDate { get; set; }

    }
}
