using LiteBus.Queries.Abstractions;
using YssWebstoreApi.Models.DTOs.Accounts;
using YssWebstoreApi.Persistance.Repositories.Interfaces;

namespace YssWebstoreApi.Features.Accounts.Queries
{
    public class GetAccountByIdQueryHandler
        : IQueryHandler<GetAccountByIdQuery, Result<AccountResponse>>
    {
        private readonly IAccountRepository _accountRepository;

        public GetAccountByIdQueryHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<Result<AccountResponse>> HandleAsync(GetAccountByIdQuery message, CancellationToken cancellationToken = default)
        {
            var account = await _accountRepository.GetAsync(message.AccountId);

            if (account is null)
            {
                return CommonErrors.ResourceNotFound;
            }

            return new AccountResponse
            {
                UniqueName = account.UniqueName,
                DisplayName = account.DisplayName,
                StatusText = account.StatusText
            };
        }
    }
}
