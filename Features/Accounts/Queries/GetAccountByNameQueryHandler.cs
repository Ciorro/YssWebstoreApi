using LiteBus.Queries.Abstractions;
using YssWebstoreApi.Api.DTO.Accounts;
using YssWebstoreApi.Persistance.Repositories.Interfaces;
using YssWebstoreApi.Persistance.Storage.Interfaces;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Accounts.Queries
{
    public class GetAccountByNameQueryHandler
        : IQueryHandler<GetAccountByNameQuery, Result<AccountResponse>>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IStorage _storage;

        public GetAccountByNameQueryHandler(IAccountRepository accountRepository, IStorage storage)
        {
            _accountRepository = accountRepository;
            _storage = storage;
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
                Id = account.Id,
                UniqueName = account.UniqueName,
                DisplayName = account.DisplayName,
                StatusText = account.StatusText,
                AvatarUrl = _storage.GetUrl(account.Avatar?.Path!)
            };
        }
    }
}
