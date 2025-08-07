using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Auth
{
    public static class AuthErrors
    {
        public static readonly Error BadCredentials =
            new Error(ErrorHelper.GetName(), "The Email or Password was incorrect.");

        public static readonly Error AccessDenied =
            new Error(ErrorHelper.GetName(), "You don't have access to this resource.");
    }
}
