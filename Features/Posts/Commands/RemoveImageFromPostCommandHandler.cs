using LiteBus.Commands.Abstractions;
using YssWebstoreApi.Entities;
using YssWebstoreApi.Features.Auth;
using YssWebstoreApi.Persistance.Repositories.Interfaces;
using YssWebstoreApi.Persistance.Storage.Interfaces;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Posts.Commands
{
    public class RemoveImageFromPostCommandHandler
        : ICommandHandler<RemoveImageFromPostCommand, Result>
    {
        private readonly IRepository<Post> _postRepository;
        private readonly IStorage _storage;

        public RemoveImageFromPostCommandHandler(IRepository<Post> postRepository, IStorage storage)
        {
            _postRepository = postRepository;
            _storage = storage;
        }

        public async Task<Result> HandleAsync(RemoveImageFromPostCommand message, CancellationToken cancellationToken = default)
        {
            var post = await _postRepository.GetAsync(message.PostId);

            if (post is null || post.Image is null)
            {
                return CommonErrors.ResourceNotFound;
            }
            if (post.AccountId != message.AccountId)
            {
                return AuthErrors.AccessDenied;
            }

            await _storage.Delete(post.Image.Path);
            post.Image = null;
            await _postRepository.UpdateAsync(post);
            return Result.Ok();
        }
    }
}
