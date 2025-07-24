namespace YssWebstoreApi.Persistance.Storage.Interfaces
{
    public interface IStorage
    {
        Task Upload(string path, Stream stream);
        Task Delete(string path);
    }
}
