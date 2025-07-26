using Slugify;
using System.Text;
using YssWebstoreApi.Security;

namespace YssWebstoreApi.Extensions
{
    public static class StringExtensions
    {
        private static readonly SlugHelper SlugHelper = new();

        public static string ToUniqueSlug(this string inputString, ushort suffixLength = 6)
        {
            var slugBuilder = new StringBuilder(
                SlugHelper.GenerateSlug(inputString));
            slugBuilder.Append('-');
            slugBuilder.Append(SecurityUtils.GetRandomString(suffixLength).ToLower());

            return slugBuilder.ToString();
        }
    }
}
