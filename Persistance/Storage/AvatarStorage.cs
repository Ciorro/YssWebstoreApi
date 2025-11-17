using YssWebstoreApi.Entities;
using YssWebstoreApi.Helpers;
using YssWebstoreApi.Persistance.Storage.Interfaces;

namespace YssWebstoreApi.Persistance.Storage
{
    public class AvatarStorage : IAvatarStorage
    {
        const string Directory = "accounts";
        const string FileName = "avatar.jpg";

        private readonly IImageStorage _imageStorage;
        private readonly TimeProvider _timeProvider;

        public AvatarStorage(IImageStorage imageStorage, TimeProvider timeProvider)
        {
            _imageStorage = imageStorage;
            _timeProvider = timeProvider;
        }

        public async Task<Resource> UploadAvatar(Guid accountId, IFormFile file)
        {
            string path = PathHelper.UnixCombine(
                Directory,
                accountId.ToString(),
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
