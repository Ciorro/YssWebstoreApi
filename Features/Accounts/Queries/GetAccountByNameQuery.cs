using LiteBus.Queries.Abstractions;
using YssWebstoreApi.Api.DTO.Accounts;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Accounts.Queries
{
    public class GetAccountByNameQuery(string uniqueName) : IQuery<Result<AccountResponse>>
    {
        public string UniqueName { get; set; } = uniqueName;
        public Guid? FollowedBy { get; set; }
    }
}
