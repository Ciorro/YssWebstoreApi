using YssWebstoreApi.Models;

namespace YssWebstoreApi.Repositories.Abstractions
{
    public interface ICredentialRepository : IRepository<Credentials>
    {
        Task<Credentials?> GetByEmailAsync(string email);
        Task<Credentials?> GetByAccountAsync(uint accountId);
    }
}
