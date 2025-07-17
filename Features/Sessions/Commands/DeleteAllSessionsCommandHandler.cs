using LiteBus.Commands.Abstractions;
using YssWebstoreApi.Services;

namespace YssWebstoreApi.Features.Sessions.Commands
{
    public class DeleteAllSessionsCommandHandler
        : ICommandHandler<DeleteAllSessionsCommand, Result>
    {
        private readonly ISessionServiceOld _sessionService;

        public DeleteAllSessionsCommandHandler(ISessionServiceOld sessionService)
        {
            _sessionService = sessionService;
        }

        public async Task<Result> HandleAsync(DeleteAllSessionsCommand message, CancellationToken cancellationToken = default)
        {
            if (await _sessionService.DeleteAllSessions(message.AccountId))
            {
                return Result.Ok();
            }

            return CommonErrors.ResourceNotFound;
        }
    }
}
