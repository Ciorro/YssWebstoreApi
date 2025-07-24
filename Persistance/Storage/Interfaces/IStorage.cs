namespace YssWebstoreApi.Persistance.Storage.Interfaces
{
    public interface IStorage
    {
        string? GetUrl(string path);
        Task Upload(string path, Stream stream);
        Task Delete(string path);
    }
}
