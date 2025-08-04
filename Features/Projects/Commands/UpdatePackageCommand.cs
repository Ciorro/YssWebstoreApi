using LiteBus.Commands.Abstractions;
using System.Windows.Input;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Projects.Commands
{
    public class UpdatePackageCommand : ICommand<Result>
    {
        public required Guid AccountId { get; init; }
        public required Guid ProjectId { get; init; }
        public required Guid PackageId { get; init; }
        public required string Name { get; init; }
    }
}
