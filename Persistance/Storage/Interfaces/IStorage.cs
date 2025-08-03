namespace YssWebstoreApi.Persistance.Storage.Interfaces
{
    public interface IStorage
    {
        Task<string?> GetPublicUrl(string bucketId, string path);
        Task<string?> GetPrivateUrl(string bucketId, string path, TimeSpan expiresIn);
        Task<string?> Upload(string bucketId, string path, Stream stream);
        Task Delete(string bucketId, string path);
    }
}
