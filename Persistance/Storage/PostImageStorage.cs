using YssWebstoreApi.Entities;
using YssWebstoreApi.Helpers;
using YssWebstoreApi.Persistance.Storage.Interfaces;

namespace YssWebstoreApi.Persistance.Storage
{
    public class PostImageStorage : IPostImageStorage
    {
        const string Directory = "posts";
        const string FileName = "cover.jpg";

        private readonly IImageStorage _imageStorage;
        private readonly TimeProvider _timeProvider;

        public PostImageStorage(IImageStorage imageStorage, TimeProvider timeProvider)
        {
            _imageStorage = imageStorage;
            _timeProvider = timeProvider;
        }

        public async Task<Resource> UploadCoverImage(Guid postId, IFormFile file)
        {
            string path = PathHelper.UnixCombine(
                Directory,
                postId.ToString(),
                FileName);

            string? url = await _imageStorage.Upload(path, file);

            var creationTime = _timeProvider.GetUtcNow().UtcDateTime;
            var id = Guid.CreateVersion7(creationTime);

            return new Resource
            {
                Id = id,
                CreatedAt = creationTime,
                UpdatedAt = creationTime,
                Path = path,
                PublicUrl = url
            };
        }
    }
}
