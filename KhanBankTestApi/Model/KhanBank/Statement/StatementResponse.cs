using System.Security.Principal;

namespace KhanBankTestApi.Model.KhanBank.Statement
{
    public class StatementResponse
    {
        public List<StatementReturnModel>? transactions { get; set; }
        public string? account { get; set; }
        public string? iban { get; set; }
        public string? currency { get; set; }
        public string? customerName { get; set; }
        public string? productName { get; set; }
        public string? branch { get; set; }
        public string? error { get; set; }
    }
}
