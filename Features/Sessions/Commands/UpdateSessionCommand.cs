using LiteBus.Commands.Abstractions;
using YssWebstoreApi.Models.DTOs.Auth;

namespace YssWebstoreApi.Features.Sessions.Commands
{
    public class UpdateSessionCommand : ICommand<Result<TokenCredentials>>
    {
        public required Guid AccountId { get; set; }
        public required string SessionToken { get; set; }
    }
}
