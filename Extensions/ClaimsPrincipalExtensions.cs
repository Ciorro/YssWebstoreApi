using System.Security.Claims;

namespace YssWebstoreApi.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static bool TryGetUserId(this ClaimsPrincipal claimsPrincipal, out Guid id)
        {
            return Guid.TryParse(claimsPrincipal.FindFirst("accountId")?.Value, out id);
        }
    }
}
