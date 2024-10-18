using MediatR;
using YssWebstoreApi.Models;
using YssWebstoreApi.Repositories.Abstractions;

namespace YssWebstoreApi.Features.Commands.Accounts
{
    public class UpdateAccountCommandHandler : IRequestHandler<UpdateAccountCommand, ulong?>
    {
        private readonly IRepository<Account> _accounts;

        public UpdateAccountCommandHandler(IRepository<Account> accounts)
        {
            _accounts = accounts;
        }

        public async Task<ulong?> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
        {
            return await _accounts.UpdateAsync(request.AccountId, new Account
            {
                UniqueName = request.UniqueName,
                DisplayName = request.DisplayName,
                Status = request.Status
            });
        }
    }
}
