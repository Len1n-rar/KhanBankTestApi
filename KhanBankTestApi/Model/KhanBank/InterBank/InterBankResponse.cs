namespace KhanBankTestApi.Model.KhanBank.InterBank
{
    public class InterBankResponse
    {
        public string? uuid { get; set; }
        public decimal journalNo { get; set; }
        public string? systemDate { get; set; }
        public string? error { get; set; }
        public InterBankReturnModel? bancsTransaction { get; set; }
    }
}
