using MediatR;
using YssWebstoreApi.Models.DTOs.Accounts;

namespace YssWebstoreApi.Features.Queries.Accounts
{
    public class GetPublicAccountByIdQuery(ulong accountId) : IRequest<PublicAccount?>
    {
        public ulong AccountId { get; } = accountId;
    }
}
