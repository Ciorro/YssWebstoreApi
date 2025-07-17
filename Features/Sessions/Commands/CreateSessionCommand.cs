using LiteBus.Commands.Abstractions;
using YssWebstoreApi.Features.Auth.Common;
using YssWebstoreApi.Models.DTOs.Auth;

namespace YssWebstoreApi.Features.Sessions.Commands
{
    public class CreateSessionCommand : AuthenticatedCommand, ICommand<Result<TokenCredentials>>
    {
        public string? DeviceInfo { get; set; }
    }
}
