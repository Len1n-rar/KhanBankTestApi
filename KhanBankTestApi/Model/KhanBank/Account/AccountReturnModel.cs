namespace KhanBankTestApi.Model.KhanBank.Account
{
    public class AccountReturnModel
    {
        public string? name { get; set; }
        public string? number { get; set; }
        public decimal balance { get; set; }
        public string? currency { get; set; }
        public string? ibanAcctNo { get; set; }
        public decimal holdBalance { get; set; }
        public decimal availableBalance { get; set; }
    }
}
