using LiteBus.Commands.Abstractions;
using YssWebstoreApi.Persistance.Repositories.Interfaces;
using YssWebstoreApi.Security;

namespace YssWebstoreApi.Features.Accounts.Commands
{
    public class CreateVerificationCodeCommandHandler
        : ICommandHandler<CreateVerificationCodeCommand, Result<string>>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IConfiguration _configuration;
        private readonly TimeProvider _timeProvider;

        public CreateVerificationCodeCommandHandler(IAccountRepository accountRepository, IConfiguration configuration, TimeProvider timeProvider)
        {
            _accountRepository = accountRepository;
            _configuration = configuration;
            _timeProvider = timeProvider;
        }

        public async Task<Result<string>> HandleAsync(CreateVerificationCodeCommand message, CancellationToken cancellationToken = default)
        {
            var account = await _accountRepository.GetAsync(message.AccountId);

            if (account is null || account.Credentials.IsVerified)
            {
                return CommonErrors.ResourceNotFound;
            }
            if (account.Credentials.IsVerified)
            {
                return AccountErrors.AlreadyVerified;
            }

            var currentTime = _timeProvider.GetUtcNow().UtcDateTime;
            var codeLifetime = _configuration.GetValue<TimeSpan?>("Security:VerificationCodeLifetime")
                ?? TimeSpan.FromMinutes(10);

            account.Credentials.VerificationCode = SecurityUtils.GetRandomString(32);
            account.Credentials.VerificationCodeExpiresAt = currentTime.Add(codeLifetime);

            await _accountRepository.UpdateAsync(account);
            return account.Credentials.VerificationCode;
        }
    }
}
