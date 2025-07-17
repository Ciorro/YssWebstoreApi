namespace YssWebstoreApi.Persistance.Storage
{
    public interface IFileStorage
    {
        Task<byte[]> GetData(string path);
        Task<Stream> GetStream(string path);
        Task Create(string path, byte[] data);
        Task Create(string path, Stream stream);
        Task Delete(string path);
        string GetUrl(string path);
    }
}
