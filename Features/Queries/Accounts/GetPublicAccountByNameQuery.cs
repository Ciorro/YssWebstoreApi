using MediatR;
using YssWebstoreApi.Models.DTOs.Accounts;

namespace YssWebstoreApi.Features.Queries.Accounts
{
    public class GetPublicAccountByNameQuery(string uniqueName) : IRequest<PublicAccount?>
    {
        public string UniqueName { get; } = uniqueName;
    }
}
