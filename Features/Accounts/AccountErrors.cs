using System.Net;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Accounts
{
    public static class AccountErrors
    {
        public static readonly Error InvalidVerificationCode =
            new Error(ErrorHelper.GetName(), (int)HttpStatusCode.Unauthorized, "Invalid or expired verification code.");

        public static readonly Error AlreadyVerified =
            new Error(ErrorHelper.GetName(), (int)HttpStatusCode.Conflict, "This account has already been verified.");

        public static readonly Error FollowedSelf =
            new Error(ErrorHelper.GetName(), (int)HttpStatusCode.Conflict, "You cannot follow yourself.");

        public static readonly Error AlreadyFollowed =
            new Error(ErrorHelper.GetName(), (int)HttpStatusCode.Conflict, "You are already following that account.");
    }
}
