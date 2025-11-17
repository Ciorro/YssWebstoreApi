using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using YssWebstoreApi.Extensions;
using YssWebstoreApi.Persistance.Storage.Interfaces;

namespace YssWebstoreApi.Persistance.Storage
{
    public class ImageStorage : IImageStorage
    {
        const string ImagesBucketId = "images";
        private readonly IStorage _storage;

        public ImageStorage(IStorage storage)
        {
            _storage = storage;
        }

        public async Task<string?> Upload(string path, IFormFile file)
        {
            using (var image = await Image.LoadAsync(file.OpenReadStream()))
            using (var ms = new MemoryStream())
            {
                await image.SaveAsync(ms, image.DetectEncoder(path));
                return await _storage.Upload(ImagesBucketId, path, ms.ToArray());
            }
        }

        public async Task Delete(string path)
        {
            await _storage.Delete(ImagesBucketId, path);
        }
    }
}
