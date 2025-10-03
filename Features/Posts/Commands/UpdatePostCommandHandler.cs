using LiteBus.Commands.Abstractions;
using YssWebstoreApi.Entities;
using YssWebstoreApi.Features.Auth;
using YssWebstoreApi.Persistance.Repositories.Interfaces;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Posts.Commands
{
    public class UpdatePostCommandHandler
        : ICommandHandler<UpdatePostCommand, Result>
    {
        private readonly IRepository<Post> _postRepository;
        private readonly TimeProvider _timeProvider;

        public UpdatePostCommandHandler(IRepository<Post> postRepository, TimeProvider timeProvider)
        {
            _postRepository = postRepository;
            _timeProvider = timeProvider;
        }

        public async Task<Result> HandleAsync(UpdatePostCommand message, CancellationToken cancellationToken = default)
        {
            var post = await _postRepository.GetAsync(message.PostId);

            if (post is null)
            {
                return CommonErrors.ResourceNotFound;
            }
            if (post.AccountId != message.AccountId)
            {
                return AuthErrors.AccessDenied;
            }

            post.UpdatedAt = _timeProvider.GetUtcNow().UtcDateTime;
            post.Title = message.Title;
            post.Content = message.Content;
            post.TargetProjectId = message.TargetProjectId;

            await _postRepository.UpdateAsync(post);
            return Result.Ok();
        }
    }
}
