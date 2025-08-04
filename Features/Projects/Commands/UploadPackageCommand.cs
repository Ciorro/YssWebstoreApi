using LiteBus.Commands.Abstractions;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Projects.Commands
{
    public class UploadPackageCommand : ICommand<Result<Guid>>
    {
        public required Guid AccountId { get; init; }
        public required Guid ProjectId { get; init;}
        public required IFormFile File { get; init;}
        public required string Name { get; init; }
        public required string Version { get; init; }
        public required Entities.OperatingSystem TargetOS { get; init; }
    }
}
