using LiteBus.Commands.Abstractions;
using YssWebstoreApi.Api.DTO.Auth;
using YssWebstoreApi.Features.Auth;
using YssWebstoreApi.Persistance.Repositories.Interfaces;
using YssWebstoreApi.Services;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Sessions.Commands
{
    public class CreateSessionCommandHandler
        : ICommandHandler<CreateSessionCommand, ValueResult<TokenCredentials>>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ISessionService _sessionService;

        public CreateSessionCommandHandler(IAccountRepository accountRepository, ISessionService sessionService)
        {
            _accountRepository = accountRepository;
            _sessionService = sessionService;
        }

        public async Task<ValueResult<TokenCredentials>> HandleAsync(CreateSessionCommand message, CancellationToken cancellationToken = default)
        {
            if (!message.TryGetAuthenticatedAccount(out var account))
            {
                return AuthErrors.BadCredentials;
            }

            TokenCredentials tokens = _sessionService.CreateSession(account, message.DeviceInfo);
            await _accountRepository.UpdateAsync(account);
            return tokens;
        }
    }
}
