using LiteBus.Commands.Abstractions;
using YssWebstoreApi.Entities;
using YssWebstoreApi.Features.Auth;
using YssWebstoreApi.Persistance.Repositories.Interfaces;
using YssWebstoreApi.Persistance.Storage.Interfaces;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Projects.Commands
{
    public class DeletePackageCommandHandler
        : ICommandHandler<DeletePackageCommand, Result>
    {
        private readonly IRepository<Project> _projectRepository;
        private readonly IProjectStorage _projectStorage;

        public DeletePackageCommandHandler(IRepository<Project> projectRepository, IProjectStorage projectStorage)
        {
            _projectRepository = projectRepository;
            _projectStorage = projectStorage;
        }

        public async Task<Result> HandleAsync(DeletePackageCommand message, CancellationToken cancellationToken = default)
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

            var package = project.Packages.SingleOrDefault(x => x.Id == message.PackageId);
            if (package is null)
            {
                return CommonErrors.ResourceNotFound;
            }

            project.Packages.Remove(package);
            project.RegenerateOSTags();

            await _projectStorage.DeletePackage(package.Path);
            await _projectRepository.UpdateAsync(project);
            return Result.Ok();
        }
    }
}
