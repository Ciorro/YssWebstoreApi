using LiteBus.Commands.Abstractions;
using YssWebstoreApi.Api.DTO.Auth;
using YssWebstoreApi.Persistance.Repositories.Interfaces;
using YssWebstoreApi.Services;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Sessions.Commands
{
    public class UpdateSessionCommandHandler
        : ICommandHandler<UpdateSessionCommand, ValueResult<TokenCredentials>>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ISessionService _sessionService;

        public UpdateSessionCommandHandler(IAccountRepository accountRepository, ISessionService sessionService)
        {
            _accountRepository = accountRepository;
            _sessionService = sessionService;
        }

        public async Task<ValueResult<TokenCredentials>> HandleAsync(UpdateSessionCommand message, CancellationToken cancellationToken = default)
        {
            var account = await _accountRepository.GetAsync(message.AccountId);
            var session = account?.Sessions.FirstOrDefault(x => x.SessionToken == message.SessionToken);

            if (session is null)
            {
                return SessionErrors.SessionNotFound;
            }

            TokenCredentials? tokens = _sessionService.UpdateSession(account!, session);
            if (tokens is null)
            {
                return SessionErrors.SessionExpired;
            }

            await _accountRepository.UpdateAsync(account!);
            return tokens;
        }
    }
}
