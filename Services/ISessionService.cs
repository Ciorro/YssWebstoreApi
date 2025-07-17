using YssWebstoreApi.Models;
using YssWebstoreApi.Models.DTOs.Auth;

namespace YssWebstoreApi.Services
{
    public interface ISessionService
    {
        TokenCredentials CreateSession(Account account, string? deviceInfo);
        TokenCredentials? UpdateSession(Account account, Session session);
    }
}