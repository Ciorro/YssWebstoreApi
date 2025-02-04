using MediatR;
using YssWebstoreApi.Models;
using YssWebstoreApi.Repositories.Abstractions;

namespace YssWebstoreApi.Features.Commands.Packages
{
    public class DeletePackageCommandHandler : IRequestHandler<DeletePackageCommand, bool>
    {
        private readonly IRepository<Package> _packages;

        public DeletePackageCommandHandler(IRepository<Package> packages)
        {
            _packages = packages;
        }

        public async Task<bool> Handle(DeletePackageCommand request, CancellationToken cancellationToken)
        {
            return await _packages.DeleteAsync(request.PackageId);
        }
    }
}
