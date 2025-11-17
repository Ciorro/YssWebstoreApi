namespace YssWebstoreApi.Persistance.Storage.Interfaces
{
    public interface IImageStorage
    {
        Task<string?> Upload(string path, IFormFile file);
        Task Delete(string path);
    }
}
