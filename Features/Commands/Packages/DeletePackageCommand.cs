using MediatR;

namespace YssWebstoreApi.Features.Commands.Packages
{
    public class DeletePackageCommand(ulong packageId) : IRequest<bool>
    {
        public ulong PackageId { get; } = packageId;
    }
}
