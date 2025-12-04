using LiteBus.Commands.Abstractions;
using YssWebstoreApi.Entities;
using YssWebstoreApi.Features.Auth;
using YssWebstoreApi.Persistance.Repositories.Interfaces;
using YssWebstoreApi.Persistance.Storage.Interfaces;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Projects.Commands
{
    public class DeleteBannerCommandHandler
        : ICommandHandler<DeleteBannerCommand, Result>
    {
        private readonly IRepository<Project> _projectRepository;
        private readonly IImageStorage _imageStorage;

        public DeleteBannerCommandHandler(IRepository<Project> projectRepository, IImageStorage imageStorage)
        {
            _projectRepository = projectRepository;
            _imageStorage = imageStorage;
        }

        public async Task<Result> HandleAsync(DeleteBannerCommand message, CancellationToken cancellationToken = default)
        {
            var project = await _projectRepository.GetAsync(message.ProjectId);

            if (project is null || project.Banner is null)
            {
                return CommonErrors.ResourceNotFound;
            }
            if (project.AccountId != message.AccountId)
            {
                return AuthErrors.AccessDenied;
            }

            await _imageStorage.Delete(project.Banner.Path);
            project.Banner = null;
            await _projectRepository.UpdateAsync(project);
            return Result.Ok();
        }
    }
}
