using LiteBus.Queries.Abstractions;
using YssWebstoreApi.Models.DTOs.Accounts;
using YssWebstoreApi.Persistance.Repositories.Interfaces;

namespace YssWebstoreApi.Features.Accounts.Queries
{
    public class GetAccountByNameQueryHandler
        : IQueryHandler<GetAccountByNameQuery, Result<AccountResponse>>
    {
        private readonly IAccountRepository _accountRepository;

        public GetAccountByNameQueryHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<Result<AccountResponse>> HandleAsync(GetAccountByNameQuery message, CancellationToken cancellationToken = default)
        {
            var account = await _accountRepository.GetByUniqueNameAsync(message.UniqueName);

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
