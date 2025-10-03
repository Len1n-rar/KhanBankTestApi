using System.Security.Principal;

namespace KhanBankTestApi.Model.KhanBank.Account
{
    public class AccountsResponse
    {
        public List<AccountReturnModel>? accounts { get; set; }
        public string? error { get; set; }
    }
}
