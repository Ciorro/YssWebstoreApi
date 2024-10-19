using MediatR;
using YssWebstoreApi.Models;
using YssWebstoreApi.Repositories.Abstractions;

namespace YssWebstoreApi.Features.Commands.Packages
{
    public class UpdatePackageCommandHandler : IRequestHandler<UpdatePackageCommand, ulong?>
    {
        private readonly IRepository<Package> _packages;

        public UpdatePackageCommandHandler(IRepository<Package> packages)
        {
            _packages = packages;
        }

        public async Task<ulong?> Handle(UpdatePackageCommand request, CancellationToken cancellationToken)
        {
            return await _packages.UpdateAsync(request.PackageId, new Package
            {
                Name = request.Name
            });
        }
    }
}
