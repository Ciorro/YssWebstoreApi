


namespace YssWebstoreApi.Services.Files
{
    public class PhysicalFileSystem : IFilesystemService
    {
        public string Root { get; }

        public PhysicalFileSystem(string root)
        {
            Root = root;
        }

        public Task<bool> Exists(string path)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Create(string path)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(string path)
        {
            throw new NotImplementedException();
        }

        public Task<Stream> Open(string path)
        {
            throw new NotImplementedException();
        }
    }
}
