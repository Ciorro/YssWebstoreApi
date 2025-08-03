using LiteBus.Commands.Abstractions;
using YssWebstoreApi.Entities;
using YssWebstoreApi.Features.Auth;
using YssWebstoreApi.Persistance.Repositories.Interfaces;
using YssWebstoreApi.Persistance.Storage.Interfaces;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Projects.Commands
{
    public class DeleteImageCommandHandler
        : ICommandHandler<DeleteImageCommand, Result>
    {
        private readonly IRepository<Project> _projectRepository;
        private readonly IImageStorage _imageStorage;

        public DeleteImageCommandHandler(IRepository<Project> projectRepository, IImageStorage imageStorage)
        {
            _projectRepository = projectRepository;
            _imageStorage = imageStorage;
        }

        public async Task<Result> HandleAsync(DeleteImageCommand message, CancellationToken cancellationToken = default)
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

            var imageToDelete = project.Images.SingleOrDefault(
                x => x.Id == message.ImageId);

            if (imageToDelete is null)
            {
                return CommonErrors.ResourceNotFound;
            }

            await _imageStorage.Delete(imageToDelete.Path);
            project.Images.Remove(imageToDelete);
            await _projectRepository.UpdateAsync(project);
            return Result.Ok();
        }
    }
}
