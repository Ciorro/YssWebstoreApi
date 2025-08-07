using LiteBus.Commands.Abstractions;
using YssWebstoreApi.Entities;
using YssWebstoreApi.Extensions;
using YssWebstoreApi.Persistance.Repositories.Interfaces;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Projects.Commands
{
    public class CreateProjectCommandHandler
        : ICommandHandler<CreateProjectCommand, Result<Guid>>
    {
        private readonly IRepository<Project> _projectRepository;
        private readonly TimeProvider _timeProvider;

        public CreateProjectCommandHandler(IRepository<Project> projectRepository, TimeProvider timeProvider)
        {
            _projectRepository = projectRepository;
            _timeProvider = timeProvider;
        }

        public async Task<Result<Guid>> HandleAsync(CreateProjectCommand message, CancellationToken cancellationToken = default)
        {
            var creationTime = _timeProvider.GetUtcNow().UtcDateTime;

            var project = new Project
            {
                Id = Guid.CreateVersion7(creationTime),
                CreatedAt = creationTime,
                UpdatedAt = creationTime,
                ReleasedAt = creationTime,
                AccountId = message.AccountId,
                Name = message.Name,
                Slug = message.Name.ToUniqueSlug(),
                Description = message.Description,
                Tags = message.Tags
            };

            await _projectRepository.InsertAsync(project);
            return project.Id;
        }
    }
}
