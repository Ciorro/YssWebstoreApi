using LiteBus.Queries.Abstractions;
using YssWebstoreApi.Models.DTOs.Accounts;

namespace YssWebstoreApi.Features.Accounts.Queries
{
    public class GetAccountByIdQuery(Guid id) : IQuery<Result<AccountResponse>>
    {
        public Guid AccountId { get; set; } = id;
    }
}
