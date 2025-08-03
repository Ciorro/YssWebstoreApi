using YssWebstoreApi.Persistance.Storage.Images;

namespace YssWebstoreApi.Persistance.Storage.Interfaces
{
    public interface IImageStorage
    {
        Task<string?> Upload(string path, IFormFile file, string format);
        Task<string?> Upload(string path, IFormFile file, ImageProperties properties);
        Task Delete(string path);
    }
}
