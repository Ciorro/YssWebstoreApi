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
            var package = await _packages.GetAsync(request.PackageId);
            if (package is not null)
            {
                package.Name = request.Name;
                return await _packages.UpdateAsync(package);
            }

            //TODO: return error
            return null;
        }
    }
}
