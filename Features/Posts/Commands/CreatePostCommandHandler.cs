using LiteBus.Commands.Abstractions;
using YssWebstoreApi.Entities;
using YssWebstoreApi.Features.Auth;
using YssWebstoreApi.Persistance.Repositories.Interfaces;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Posts.Commands
{
    public class CreatePostCommandHandler
        : ICommandHandler<CreatePostCommand, ValueResult<Guid>>
    {
        private readonly IRepository<Post> _postRepository;
        private readonly IRepository<Project> _projectRepository;
        private readonly TimeProvider _timeProvider;

        public CreatePostCommandHandler(IRepository<Post> postRepository, IRepository<Project> projectRepository, TimeProvider timeProvider)
        {
            _postRepository = postRepository;
            _projectRepository = projectRepository;
            _timeProvider = timeProvider;
        }

        public async Task<ValueResult<Guid>> HandleAsync(CreatePostCommand message, CancellationToken cancellationToken = default)
        {
            if (message.TargetProjectId.HasValue)
            {
                var project = await _projectRepository.GetAsync(message.TargetProjectId.Value);
                
                if (project == null)
                {
                    return CommonErrors.ResourceNotFound;
                }
                if (project.AccountId != message.AccountId)
                {
                    return AuthErrors.AccessDenied;
                }
            }

            var creationDate = _timeProvider.GetUtcNow().UtcDateTime;
            var id = Guid.CreateVersion7(creationDate);

            var post = new Post()
            {
                Id = id,
                CreatedAt = creationDate,
                UpdatedAt = creationDate,
                AccountId = message.AccountId,
                Title = message.Title,
                Content = message.Content,
                TargetProjectId = message.TargetProjectId
            };

            await _postRepository.InsertAsync(post);
            return post.Id;
        }
    }
}
