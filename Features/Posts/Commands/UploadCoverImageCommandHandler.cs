using LiteBus.Commands.Abstractions;
using YssWebstoreApi.Entities;
using YssWebstoreApi.Features.Auth;
using YssWebstoreApi.Persistance.Repositories.Interfaces;
using YssWebstoreApi.Persistance.Storage.Interfaces;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Posts.Commands
{
    public class UploadCoverImageCommandHandler
        : ICommandHandler<UploadCoverImageCommand, Result<string>>
    {
        private readonly IRepository<Post> _postRepository;
        private readonly IPostImageStorage _postImageStorage;

        public UploadCoverImageCommandHandler(IRepository<Post> postRepository, IPostImageStorage postImageStorage)
        {
            _postRepository = postRepository;
            _postImageStorage = postImageStorage;
        }

        public async Task<Result<string>> HandleAsync(UploadCoverImageCommand message, CancellationToken cancellationToken = default)
        {
            var post = await _postRepository.GetAsync(message.PostId);

            if (post is null)
            {
                return CommonErrors.ResourceNotFound;
            }
            if (message.AccountId != post.AccountId)
            {
                return AuthErrors.AccessDenied;
            }

            Resource imageResource = await _postImageStorage.UploadCoverImage(
                post.Id, message.File);
            post.Image = imageResource;

            await _postRepository.UpdateAsync(post);
            return imageResource.PublicUrl!;
        }
    }
}
