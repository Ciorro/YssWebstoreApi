using YssWebstoreApi.Models;

namespace YssWebstoreApi.Repositories.Abstractions
{
    public interface ISessionRepository : IRepository<Session>
    {
        Task<Session?> GetSessionByToken(string token);
    }
}
