using LiteBus.Queries.Abstractions;
using YssWebstoreApi.Models.DTOs.Accounts;

namespace YssWebstoreApi.Features.Accounts.Queries
{
    public class GetAccountByNameQuery(string uniqueName) : IQuery<Result<AccountResponse>>
    {
        public string UniqueName { get; set; } = uniqueName;
    }
}
