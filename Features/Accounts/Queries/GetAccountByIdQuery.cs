using LiteBus.Queries.Abstractions;
using YssWebstoreApi.Api.DTO.Accounts;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Accounts.Queries
{
    public class GetAccountByIdQuery(Guid id) : IQuery<Result<AccountResponse>>
    {
        public Guid AccountId { get; set; } = id;
    }
}
