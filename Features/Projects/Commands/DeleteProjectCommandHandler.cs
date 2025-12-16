using LiteBus.Commands.Abstractions;
using YssWebstoreApi.Entities;
using YssWebstoreApi.Features.Auth;
using YssWebstoreApi.Persistance.Repositories.Interfaces;
using YssWebstoreApi.Persistance.Storage.Interfaces;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Projects.Commands
{
    public class DeleteProjectCommandHandler
        : ICommandHandler<DeleteProjectCommand, Result>
    {
        private readonly IRepository<Project> _projectRepository;
        private readonly IProjectStorage _projectStorage;

        public DeleteProjectCommandHandler(IRepository<Project> projectRepository, IProjectStorage projectStorage)
        {
            _projectRepository = projectRepository;
            _projectStorage = projectStorage;
        }

        public async Task<Result> HandleAsync(DeleteProjectCommand message, CancellationToken cancellationToken = default)
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

            await _projectRepository.DeleteAsync(project.Id);
            await Task.WhenAll(
                DeleteImages(project), 
                DeletePackages(project));

            return Result.Ok();
        }

        private async Task DeleteImages(Project project)
        {
            var deleteTasks = new List<Task>();

            if (project.Banner is not null)
            {
                deleteTasks.Add(_projectStorage.DeleteBanner(project.Banner.Path));
            }
            if (project.Icon is not null)
            {
                deleteTasks.Add(_projectStorage.DeleteBanner(project.Icon.Path));
            }

            deleteTasks.AddRange(project.Images.Select(
                x => _projectStorage.DeleteImage(x.Path)));

            await Task.WhenAll(deleteTasks);
        }

        private async Task DeletePackages(Project project)
        {
            await Task.WhenAll(project.Packages.Select(
                x => _projectStorage.DeletePackage(x.Path)));
        }
    }
}
