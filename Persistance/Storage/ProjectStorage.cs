using YssWebstoreApi.Entities;
using YssWebstoreApi.Helpers;
using YssWebstoreApi.Persistance.Storage.Images;
using YssWebstoreApi.Persistance.Storage.Interfaces;

namespace YssWebstoreApi.Persistance.Storage
{
    public class ProjectStorage : IProjectStorage
    {
        const string PackagesBucket = "packages";
        const string ImagesDirectory = "projects";
        const string IconFileName = "icon.png";

        private readonly IImageStorage _imageStorage;
        private readonly TimeProvider _timeProvider;

        public ProjectStorage(IImageStorage imageStorage, TimeProvider timeProvider)
        {
            _imageStorage = imageStorage;
            _timeProvider = timeProvider;
        }

        public async Task<Resource> UploadIcon(Guid projectId, IFormFile file)
        {
            string path = PathHelper.UnixCombine(
                ImagesDirectory,
                projectId.ToString(),
                IconFileName);

            string? url = await _imageStorage.Upload(path, file, ImageProperties.AvatarImage);
            
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

        public async Task<Resource> UploadImage(Guid projectId, IFormFile file)
        {
            string fileName = Guid.NewGuid().ToString() + ".jpg";

            string path = PathHelper.UnixCombine(
                ImagesDirectory,
                projectId.ToString(),
                fileName);

            string? url = await _imageStorage.Upload(path, file, "jpg");

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
