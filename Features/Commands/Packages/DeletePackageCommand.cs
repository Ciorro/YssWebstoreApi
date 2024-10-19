using MediatR;

namespace YssWebstoreApi.Features.Commands.Packages
{
    public class DeletePackageCommand(ulong packageId) : IRequest<ulong?>
    {
        public ulong PackageId { get; } = packageId;
    }
}
