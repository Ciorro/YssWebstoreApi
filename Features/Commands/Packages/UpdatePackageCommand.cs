using MediatR;
using System.Diagnostics.CodeAnalysis;
using YssWebstoreApi.Models.DTOs.Package;

namespace YssWebstoreApi.Features.Commands.Packages
{
    public class UpdatePackageCommand : IRequest<ulong?>
    {
        public required ulong PackageId { get; init; }
        public required string Name { get; init; }

        public UpdatePackageCommand() { }

        [SetsRequiredMembers]
        public UpdatePackageCommand(ulong packageId, UpdatePackage updatePackage)
        {
            PackageId = packageId;
            Name = updatePackage.Name;
        }
    }
}
