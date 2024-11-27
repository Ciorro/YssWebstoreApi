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
            var account = await _accounts.GetAsync(request.AccountId);
            if (account is not null)
            {
                account.UniqueName = request.UniqueName;
                account.DisplayName = request.DisplayName;
                account.Status = request.Status;

                return await _accounts.UpdateAsync(account);
            }


            //TODO: Return error
            return null;
        }
    }
}
