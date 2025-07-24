using YssWebstoreApi.Persistance.Storage.Interfaces;

namespace YssWebstoreApi.Extensions
{
    public static class StorageExtensions
    {
        public static Task Upload(this IStorage storage, string path, byte[] data)
        {
            using (var ms = new MemoryStream(data))
            {
                return storage.Upload(path, ms);
            }
        }
    }
}
