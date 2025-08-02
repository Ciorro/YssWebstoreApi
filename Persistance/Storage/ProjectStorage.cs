using YssWebstoreApi.Helpers;
using YssWebstoreApi.Persistance.Storage.Images;
using YssWebstoreApi.Persistance.Storage.Interfaces;

namespace YssWebstoreApi.Persistance.Storage
{
    public class ProjectStorage : IProjectStorage
    {
        const string Directory = "Projects";
        const string IconFileName = "icon.png";

        private readonly IImageStorage _imageStorage;

        public ProjectStorage(IImageStorage imageStorage)
        {
            _imageStorage = imageStorage;
        }

        public async Task<string> UploadIcon(Guid projectId, IFormFile file)
        {
            string path = PathHelper.UnixCombine(
                Directory,
                projectId.ToString(),
                IconFileName);

            await _imageStorage.Upload(path, file, ImageProperties.AvatarImage);
            return path;
        }

        public async Task<string> UploadImage(Guid projectId, IFormFile file)
        {
            string fileName = Guid.NewGuid().ToString() + ".jpg";

            string path = PathHelper.UnixCombine(
                Directory,
                projectId.ToString(),
                fileName);

            await _imageStorage.Upload(path, file, "jpg");
            return path;
        }
    }
}
