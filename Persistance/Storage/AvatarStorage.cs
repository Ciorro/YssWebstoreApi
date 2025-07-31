using YssWebstoreApi.Helpers;
using YssWebstoreApi.Persistance.Storage.Images;
using YssWebstoreApi.Persistance.Storage.Interfaces;

namespace YssWebstoreApi.Persistance.Storage
{
    public class AvatarStorage : IAvatarStorage
    {
        const string Directory = "Accounts";
        const string FileName = "avatar.jpg";

        private readonly IImageStorage _imageStorage;

        public AvatarStorage(IImageStorage imageStorage)
        {
            _imageStorage = imageStorage;
        }

        public async Task<string> UploadAvatar(Guid accountId, IFormFile file)
        {
            string path = PathHelper.UnixCombine(
                Directory,
                accountId.ToString(),
                FileName);

            await _imageStorage.Upload(path, file, ImageProperties.AvatarImage);
            return path;
        }
    }
}
