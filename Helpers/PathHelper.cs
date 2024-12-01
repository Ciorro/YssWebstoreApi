using System.Reflection;

namespace YssWebstoreApi.Helpers
{
    public static class PathHelper
    {
        public static string GetAbsolutePathRelativeToAssembly(string path, Assembly? assembly = null)
        {
            assembly ??= Assembly.GetExecutingAssembly();
            var assemblyDir = Path.GetDirectoryName(assembly.Location);

            return Path.Combine(assemblyDir ?? "", path);
        }

        public static string GetRandomPathName(string directory, string fileExtension)
        {
            return Path.Combine(directory, Guid.NewGuid().ToString("N") + fileExtension);
        }
    }
}
