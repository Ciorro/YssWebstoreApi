using System.Runtime.CompilerServices;
using System.Text;

namespace YssWebstoreApi.Helpers
{
    public static class ErrorHelper
    {
        private const string ErrorClassSuffix = "Errors";

        public static string GetName(
            [CallerFilePath] string? callerClass = null,
            [CallerMemberName] string? callerMember = null)
        {
            string className = Path.GetFileNameWithoutExtension(callerClass)!;
            if (className.EndsWith(ErrorClassSuffix, StringComparison.CurrentCultureIgnoreCase))
            {
                className = className.Substring(0, className.Length - ErrorClassSuffix.Length);
            }

            var errorNameBuilder = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(className))
            {
                errorNameBuilder.Append(className);
                errorNameBuilder.Append('.');
            }
            errorNameBuilder.Append(callerMember);

            return errorNameBuilder.ToString();
        }
    }
}
