using System.Security.Claims;

namespace YssWebstoreApi.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static ulong GetUserId(this ClaimsPrincipal claimsPrincipal)
        {
            if (ulong.TryParse(claimsPrincipal.FindFirst("account_id")?.Value, out ulong accountId))
            {
                return accountId;
            }

            throw new UnauthorizedAccessException();
        }
    }
}
