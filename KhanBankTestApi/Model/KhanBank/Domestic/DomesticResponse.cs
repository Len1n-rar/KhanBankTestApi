namespace KhanBankTestApi.Model.KhanBank.Domestic
{
    public class DomesticResponse
    {
        public string? uuid { get; set; }
        public decimal journalNo { get; set; }
        public string? systemDate { get; set; }
        public string? error { get; set; }
        public DomesticReturnModel? bancsTransaction { get; set; }
    }
}
