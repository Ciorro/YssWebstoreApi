using LiteBus.Commands.Abstractions;
using YssWebstoreApi.Models;
using YssWebstoreApi.Persistance.Repositories.Interfaces;

namespace YssWebstoreApi.Features.Posts.Commands
{
    public class CreatePostCommandHandler 
        : ICommandHandler<CreatePostCommand, Result<Guid>>
    {
        private readonly IRepository<Post> _postRepository;
        private readonly TimeProvider _timeProvider;

        public CreatePostCommandHandler(IRepository<Post> postRepository, TimeProvider timeProvider)
        {
            _postRepository = postRepository;
            _timeProvider = timeProvider;
        }

        public async Task<Result<Guid>> HandleAsync(CreatePostCommand message, CancellationToken cancellationToken = default)
        {
            var creationDate = _timeProvider.GetUtcNow().UtcDateTime;

            var post = new Post()
            {
                Id = Guid.CreateVersion7(),
                CreatedAt = creationDate,
                UpdatedAt = creationDate,
                AccountId = message.AccountId,
                Title = message.Title,
                Content = message.Content,
                TargetProjectId = message.TargetProjectId,
                ImageResourceId = message.ImageResourceId
            };

            await _postRepository.InsertAsync(post);
            return post.Id;
        }
    }
}
