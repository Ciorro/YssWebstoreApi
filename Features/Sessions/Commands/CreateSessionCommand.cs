using LiteBus.Commands.Abstractions;
using YssWebstoreApi.Api.DTO.Auth;
using YssWebstoreApi.Features.Auth.Common;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Sessions.Commands
{
    public class CreateSessionCommand : AuthenticatedCommand, ICommand<ValueResult<TokenCredentials>>
    {
        public string? DeviceInfo { get; set; }
    }
}
