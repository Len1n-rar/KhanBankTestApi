namespace KhanBankTestApi.Model.KhanBank.Domestic
{
    public class DomesticReturnModel
    {
        public decimal toRate { get; set; }
        public decimal baseAmount { get; set; }
        public decimal fromRate { get; set; }
        public string? isser { get; set; }

    }
}
