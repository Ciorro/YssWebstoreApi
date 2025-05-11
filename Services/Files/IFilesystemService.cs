namespace YssWebstoreApi.Services.Files
{
    public interface IFilesystemService
    {
        Task<bool> Exists(string path);
        Task<bool> Create(string path);
        Task<bool> Delete(string path);
        Task<Stream> Open(string path);
    }
}
