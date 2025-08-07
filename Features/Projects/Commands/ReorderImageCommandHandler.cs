using LiteBus.Commands.Abstractions;
using YssWebstoreApi.Entities;
using YssWebstoreApi.Features.Auth;
using YssWebstoreApi.Persistance.Repositories.Interfaces;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Projects.Commands
{
    public class ReorderImageCommandHandler
        : ICommandHandler<ReorderImageCommand, Result>
    {
        private readonly IRepository<Project> _projectRepository;

        public ReorderImageCommandHandler(IRepository<Project> projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<Result> HandleAsync(ReorderImageCommand message, CancellationToken cancellationToken = default)
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

            var reorderedImage = project.Images.SingleOrDefault(i => i.Id == message.ImageId);
            if (reorderedImage is null)
            {
                return CommonErrors.ResourceNotFound;
            }

            int newOrder = Math.Clamp(message.NewOrder, 0, project.Images.Count - 1);
            project.Images.Remove(reorderedImage);
            project.Images.Insert(newOrder, reorderedImage);

            await _projectRepository.UpdateAsync(project);
            return Result.Ok();
        }
    }
}
