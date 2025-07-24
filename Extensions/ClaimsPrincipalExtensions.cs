using System.Security.Claims;

namespace YssWebstoreApi.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid GetAccountId(this ClaimsPrincipal claimsPrincipal)
        {
            return Guid.TryParse(claimsPrincipal.FindFirstValue("accountId"), out var accountId) ?
                accountId : throw new UnauthorizedAccessException("Missing account id.");
        }
    }
}
