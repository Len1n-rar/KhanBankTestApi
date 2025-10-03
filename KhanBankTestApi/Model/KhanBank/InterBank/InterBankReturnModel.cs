namespace KhanBankTestApi.Model.KhanBank.InterBank
{
    public class InterBankReturnModel
    {
        public decimal toRate { get; set; }
        public decimal baseAmount { get; set; }
        public decimal fromRate { get; set; }
        public string? isser { get; set; }

    }
}
