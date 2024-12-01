
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
            path = GetFullPath(path);
            return Task.FromResult(File.Exists(path));
        }

        public Stream OpenRead(string path)
        {
            path = GetFullPath(path);
            return File.OpenRead(path);
        }

        public Stream OpenWrite(string path)
        {
            path = GetFullPath(path);
            EnsureDirectoryExists(path);
            return File.OpenWrite(path);
        }

        public Task Delete(string path)
        {
            File.Delete(GetFullPath(path));
            return Task.CompletedTask;
        }

        private string GetFullPath(string path)
        {
            return Path.Combine(Root, path);
        }

        private void EnsureDirectoryExists(string path)
        {
            var fullPath = Path.Combine(Root, path);
            var directory = Path.GetDirectoryName(fullPath);

            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }
    }
}
