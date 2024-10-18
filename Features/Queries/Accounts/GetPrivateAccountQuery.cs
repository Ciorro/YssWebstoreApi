using MediatR;
using YssWebstoreApi.Models.DTOs.Accounts;

namespace YssWebstoreApi.Features.Queries.Accounts
{
    public class GetPrivateAccountQuery(ulong accountId) : IRequest<PrivateAccount?>
    {
        public ulong AccountId { get; } = accountId;
    }
}
