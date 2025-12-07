using LiteBus.Commands.Abstractions;
using YssWebstoreApi.Entities;
using YssWebstoreApi.Features.Auth;
using YssWebstoreApi.Persistance.Repositories.Interfaces;
using YssWebstoreApi.Persistance.Storage.Interfaces;
using YssWebstoreApi.Persistance.Storage.Packages;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Projects.Commands
{
    public class UploadPackageCommandHandler
        : ICommandHandler<UploadPackageCommand, ValueResult<Guid>>
    {
        private readonly IRepository<Project> _projectRepository;
        private readonly IProjectStorage _projectStorage;

        public UploadPackageCommandHandler(IRepository<Project> projectRepository, IProjectStorage projectStorage)
        {
            _projectRepository = projectRepository;
            _projectStorage = projectStorage;
        }

        public async Task<ValueResult<Guid>> HandleAsync(UploadPackageCommand message, CancellationToken cancellationToken = default)
        {
            var project = await _projectRepository.GetAsync(message.ProjectId);

            if (project is null)
            {
                return CommonErrors.ResourceNotFound;
            }
            if (project.AccountId != message.AccountId)
            {
                return AuthErrors.AccessDenied;
            }

            var packageInfo = new PackageInfo(
                message.Name,
                message.Version,
                message.TargetOS);

            Resource packageResource = await _projectStorage.UploadPackage(
                message.ProjectId, packageInfo, message.File);

            var package = new Package
            {
                Id = packageResource.Id,
                CreatedAt = packageResource.CreatedAt,
                UpdatedAt = packageResource.UpdatedAt,
                Path = packageResource.Path,
                PublicUrl = packageResource.PublicUrl,
                Name = message.Name,
                Version = message.Version,
                TargetOS = message.TargetOS,
                Size = message.File.Length
            };
            project.Packages.Add(package);
            project.RegenerateOSTags();

            await _projectRepository.UpdateAsync(project);
            return package.Id;
        }
    }
}
