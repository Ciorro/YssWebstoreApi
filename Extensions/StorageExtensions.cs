using YssWebstoreApi.Persistance.Storage.Interfaces;

namespace YssWebstoreApi.Extensions
{
    public static class StorageExtensions
    {
        public static Task<string?> Upload(this IStorage storage, string bucketId, string path, byte[] data)
        {
            using (var ms = new MemoryStream(data))
            {
                return storage.Upload(bucketId, path, ms);
            }
        }
    }
}
