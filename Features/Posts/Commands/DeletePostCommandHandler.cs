using LiteBus.Commands.Abstractions;
using YssWebstoreApi.Entities;
using YssWebstoreApi.Features.Auth;
using YssWebstoreApi.Persistance.Repositories.Interfaces;
using YssWebstoreApi.Persistance.Storage.Interfaces;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Posts.Commands
{
    public class DeletePostCommandHandler
        : ICommandHandler<DeletePostCommand, Result>
    {
        private readonly IRepository<Post> _postRepository;
        private readonly IImageStorage _imageStorage;

        public DeletePostCommandHandler(IRepository<Post> postRepository, IImageStorage imageStorage)
        {
            _postRepository = postRepository;
            _imageStorage = imageStorage;
        }

        public async Task<Result> HandleAsync(DeletePostCommand message, CancellationToken cancellationToken = default)
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

            if (post.Image is not null)
            {
                await _imageStorage.Delete(post.Image.Path);
            }

            await _postRepository.DeleteAsync(post.Id);
            return Result.Ok();
        }
    }
}
