namespace KhanBankTestApi.Model.KhanBank.InterBank
{
    public class InterBankModel
    {
        public string? fromAccount { get; set; }
        public string? toAccount { get; set; }
        public string? toCurrency { get; set; }
        public decimal amount { get; set; }
        public string? description { get; set; }
        public string? currency { get; set; }
        public string? loginName { get; set; }
        public string? tranPassword { get; set; }
        public string? transferid { get; set; }
    }
}
