namespace YssWebstoreApi.Helpers
{
    public static class PathHelper
    {
        public static string UnixCombine(string path1, string path2)
        {
            return Path.Combine(path1, path2).Replace('\\', '/');
        }

        public static string UnixCombine(string path1, string path2, string path3)
        {
            return Path.Combine(path1, path2, path3).Replace('\\', '/');
        }

        public static string UnixCombine(string path1, string path2, string path3, string path4)
        {
            return Path.Combine(path1, path2, path3, path4).Replace('\\', '/');
        }

        public static string UnixCombine(params ReadOnlySpan<string> paths)
        {
            return Path.Combine(paths).Replace('\\', '/');
        }
    }
}
