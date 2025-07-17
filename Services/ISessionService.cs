using YssWebstoreApi.Api.DTO.Auth;
using YssWebstoreApi.Entities;

namespace YssWebstoreApi.Services
{
    public interface ISessionService
    {
        TokenCredentials CreateSession(Account account, string? deviceInfo);
        TokenCredentials? UpdateSession(Account account, Session session);
    }
}