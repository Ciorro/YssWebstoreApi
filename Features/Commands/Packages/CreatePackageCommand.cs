using MediatR;
using System.Diagnostics.CodeAnalysis;
using YssWebstoreApi.Models;
using YssWebstoreApi.Models.DTOs.Package;

namespace YssWebstoreApi.Features.Commands.Packages
{
    public class CreatePackageCommand : IRequest<ulong?>
    {
        public required ulong ProductId { get; init; }
        public required string Name { get; init; }
        public required string Version { get; init; }
        public required string DownloadUrl { get; init; }
        public OS? TargetOS { get; init; }

        public CreatePackageCommand() { }

        [SetsRequiredMembers]
        public CreatePackageCommand(ulong productId, CreatePackage createPackage)
        {
            ProductId = productId;
            Name = createPackage.Name;
            Version = createPackage.Version;
            DownloadUrl = createPackage.DownloadUrl;
            TargetOS = createPackage.TargetOS;
        }
    }
}
