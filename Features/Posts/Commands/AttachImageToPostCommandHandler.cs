using LiteBus.Commands.Abstractions;
using YssWebstoreApi.Entities;
using YssWebstoreApi.Features.Auth;
using YssWebstoreApi.Persistance.Repositories.Interfaces;
using YssWebstoreApi.Persistance.Storage.Interfaces;
using YssWebstoreApi.Services;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Posts.Commands
{
    public class AttachImageToPostCommandHandler
        : ICommandHandler<AttachImageToPostCommand, Result>
    {
        private readonly IRepository<Post> _postRepository;
        private readonly IPostImageStorage _postImageStorage;
        private readonly TimeProvider _timeProvider;

        public AttachImageToPostCommandHandler(IRepository<Post> postRepository, IPostImageStorage postImageStorage, TimeProvider timeProvider)
        {
            _postRepository = postRepository;
            _postImageStorage = postImageStorage;
            _timeProvider = timeProvider;
        }

        public async Task<Result> HandleAsync(AttachImageToPostCommand message, CancellationToken cancellationToken = default)
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

            string? fileName = await _postImageStorage.UploadCoverImage(
                post.Id, message.File);

            if (string.IsNullOrEmpty(fileName))
            {
                return new Error("Upload error", "Failed to upload image file.");
            }

            var creationTime = _timeProvider.GetUtcNow().UtcDateTime;
            var id = Guid.CreateVersion7(creationTime);

            post.Image = new Resource
            {
                Id = id,
                CreatedAt = creationTime,
                UpdatedAt = creationTime,
                Path = fileName
            };

            await _postRepository.UpdateAsync(post);

            return Result.Ok();
        }
    }
}
