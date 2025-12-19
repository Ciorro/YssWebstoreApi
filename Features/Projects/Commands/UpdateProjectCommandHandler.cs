using LiteBus.Commands.Abstractions;
using YssWebstoreApi.Entities;
using YssWebstoreApi.Features.Auth;
using YssWebstoreApi.Persistance.Repositories.Interfaces;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Projects.Commands
{
    public class UpdateProjectCommandHandler
        : ICommandHandler<UpdateProjectCommand, Result>
    {
        private readonly IRepository<Project> _projectRepository;
        private readonly TimeProvider _timeProvider;

        public UpdateProjectCommandHandler(IRepository<Project> projectRepository, TimeProvider timeProvider)
        {
            _projectRepository = projectRepository;
            _timeProvider = timeProvider;
        }

        public async Task<Result> HandleAsync(UpdateProjectCommand message, CancellationToken cancellationToken = default)
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

            project.UpdatedAt = _timeProvider.GetUtcNow().UtcDateTime;
            project.Name = message.Name;
            project.Description = message.Description;

            project.Tags = message.Tags;
            project.RegenerateOSTags();

            await _projectRepository.UpdateAsync(project);
            return Result.Ok();
        }
    }
}
