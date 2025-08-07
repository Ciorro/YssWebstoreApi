using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Sessions
{
    public static class SessionErrors
    {
        public static readonly Error SessionNotFound =
            new Error(ErrorHelper.GetName(), "Session not found.");

        public static readonly Error SessionExpired =
            new Error(ErrorHelper.GetName(), "The session has expired.");
    }
}
