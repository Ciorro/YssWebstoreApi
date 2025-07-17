using LiteBus.Commands.Abstractions;

namespace YssWebstoreApi.Features.Sessions.Commands
{
    public class DeleteSessionCommand : ICommand<Result>
    {
        public required Guid AccountId { get; set; }
        public required string SessionToken { get; set; }
    }
}
