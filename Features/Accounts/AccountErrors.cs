using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Accounts
{
    public static class AccountErrors
    {
        public static readonly Error InvalidVerificationCode =
            new Error(ErrorHelper.GetName(), "Invalid or expired verification code.");

        public static readonly Error AlreadyVerified =
            new Error(ErrorHelper.GetName(), "This account has already been verified.");

        public static readonly Error FollowedSelf =
            new Error(ErrorHelper.GetName(), "You cannot follow yourself.");

        public static readonly Error AlreadyFollowed =
            new Error(ErrorHelper.GetName(), "You are already following that account.");
    }
}
