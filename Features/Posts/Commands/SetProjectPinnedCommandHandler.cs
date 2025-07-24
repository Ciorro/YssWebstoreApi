using LiteBus.Commands.Abstractions;
using YssWebstoreApi.Entities;
using YssWebstoreApi.Features.Auth;
using YssWebstoreApi.Persistance.Repositories.Interfaces;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Posts.Commands
{
    public class SetProjectPinnedCommandHandler
        : ICommandHandler<SetProjectPinnedCommand, Result>
    {
        private readonly IRepository<Project> _projectRepository;

        public SetProjectPinnedCommandHandler(IRepository<Project> projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<Result> HandleAsync(SetProjectPinnedCommand message, CancellationToken cancellationToken = default)
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

            project.IsPinned = message.IsPinned;
            await _projectRepository.UpdateAsync(project);
            return Result.Ok();
        }
    }
}
