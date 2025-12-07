using System.Net;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Sessions
{
    public static class SessionErrors
    {
        public static readonly Error SessionNotFound =
            new Error(ErrorHelper.GetName(), (int)HttpStatusCode.Unauthorized, "Session not found.");

        public static readonly Error SessionExpired =
            new Error(ErrorHelper.GetName(), (int)HttpStatusCode.Unauthorized, "The session has expired.");
    }
}
