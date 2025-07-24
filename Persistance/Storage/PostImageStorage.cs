using YssWebstoreApi.Persistance.Storage.Images;
using YssWebstoreApi.Persistance.Storage.Interfaces;

namespace YssWebstoreApi.Persistance.Storage
{
    public class PostImageStorage : IPostImageStorage
    {
        const string Directory = "Posts";
        const string FileName = "Cover.jpg";

        private readonly IImageStorage _imageStorage;

        public PostImageStorage(IImageStorage imageStorage)
        {
            _imageStorage = imageStorage;
        }

        public async Task<string> UploadCoverImage(Guid postId, IFormFile file)
        {
            string path = Path.Combine(Directory, postId.ToString(), FileName);
            await _imageStorage.Upload(path, file, ImageProperties.PostImage);
            return path;
        }
    }
}
