using YssWebstoreApi.Models;
using YssWebstoreApi.Models.DTOs.Auth;

namespace YssWebstoreApi.Services
{
    public interface ISessionServiceOld
    {
        Task<TokenCredentials> CreateSession(Guid accountId, string? deviceInfo = "");
        Task<TokenCredentials> UpdateSession(Guid accountId, string sessionToken);
        Task<bool> DeleteSession(Guid accountId, string sessionToken);
        Task<bool> DeleteAllSessions(Guid accountId);
    }
}