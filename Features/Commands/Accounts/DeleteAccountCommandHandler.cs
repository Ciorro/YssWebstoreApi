using MediatR;
using YssWebstoreApi.Models;
using YssWebstoreApi.Repositories.Abstractions;

namespace YssWebstoreApi.Features.Commands.Accounts
{
    public class DeleteAccountCommandHandler : IRequestHandler<DeleteAccountCommand, ulong?>
    {
        private readonly IRepository<Account> _accounts;

        public DeleteAccountCommandHandler(IRepository<Account> accounts)
        {
            _accounts = accounts;
        }

        public async Task<ulong?> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
        {
            //TODO: Add account cleanup?
            return await _accounts.DeleteAsync(request.AccountId);
        }
    }
}
