using System.Security.Cryptography;

namespace YssWebstoreApi.Security
{
    public static class SecurityUtils
    {
        public static string GetRandomString(ushort length)
        {
            const string chars =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            return RandomNumberGenerator.GetString(chars, length);
        }
    }
}
