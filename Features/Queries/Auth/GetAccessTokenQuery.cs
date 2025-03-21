using MediatR;
using YssWebstoreApi.Models;

namespace YssWebstoreApi.Features.Queries.Auth
{
    public class GetAccessTokenQuery(Account account) : IRequest<string?>
    {
        public Account Account { get; } = account;
    }
}
