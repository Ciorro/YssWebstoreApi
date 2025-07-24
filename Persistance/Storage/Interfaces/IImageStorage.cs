using YssWebstoreApi.Persistance.Storage.Images;

namespace YssWebstoreApi.Persistance.Storage.Interfaces
{
    public interface IImageStorage
    {
        Task Upload(string path, IFormFile file, ImageProperties properties);
    }
}
