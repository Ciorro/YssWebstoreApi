namespace YssWebstoreApi.Services.Files
{
    public interface IFilesystemService
    {
        Task<bool> Exists(string path);
        Stream OpenRead(string path);
        Stream OpenWrite(string path);
        Task Delete(string path);
    }
}
