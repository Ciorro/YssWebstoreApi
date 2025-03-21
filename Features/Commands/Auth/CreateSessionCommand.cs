using MediatR;
using YssWebstoreApi.Models;

namespace YssWebstoreApi.Features.Commands.Auth
{
    public class CreateSessionCommand(Account account) : IRequest<Session?>
    {
        public Account Account { get; } = account;
    }
}
