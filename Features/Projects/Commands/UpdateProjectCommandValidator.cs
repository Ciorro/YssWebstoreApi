using LiteBus.Commands.Abstractions;
using LiteBus.Messaging.Abstractions;
using YssWebstoreApi.Entities;
using YssWebstoreApi.Features.Tags;
using YssWebstoreApi.Persistance.Repositories.Interfaces;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Projects.Commands
{
    internal class UpdateProjectCommandValidator
        : ICommandValidator<UpdateProjectCommand>
    {
        private readonly IRepository<Project> _projectRepository;

        public UpdateProjectCommandValidator(IRepository<Project> projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task ValidateAsync(UpdateProjectCommand command, CancellationToken cancellationToken = default)
        {
            var project = await _projectRepository.GetAsync(command.ProjectId);
            if (project is not null)
            {
                var incomingProjectTypeTag = command.Tags.GetGroup("type").SingleOrDefault();
                var existingProjectTypeTag = project.Tags.GetGroup("type").SingleOrDefault();
                if (existingProjectTypeTag == incomingProjectTypeTag)
                {
                    return;
                }
            }

            AmbientExecutionContext.Current.Abort(Result.Fail(TagErrors.ProjectTypeMismatch));
        }
    }
}
