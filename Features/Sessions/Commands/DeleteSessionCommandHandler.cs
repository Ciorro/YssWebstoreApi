using LiteBus.Commands.Abstractions;
using YssWebstoreApi.Services;

namespace YssWebstoreApi.Features.Sessions.Commands
{
    public class DeleteSessionCommandHandler
        : ICommandHandler<DeleteSessionCommand, Result>
    {
        private readonly ISessionServiceOld _sessionService;

        public DeleteSessionCommandHandler(ISessionServiceOld sessionService)
        {
            _sessionService = sessionService;
        }

        public async Task<Result> HandleAsync(DeleteSessionCommand message, CancellationToken cancellationToken = default)
        {
            if (await _sessionService.DeleteSession(message.AccountId, message.SessionToken))
            {
                return Result.Ok();
            }

            return CommonErrors.ResourceNotFound;
        }
    }
}
