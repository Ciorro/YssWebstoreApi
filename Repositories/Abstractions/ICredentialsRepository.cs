using YssWebstoreApi.Models;

namespace YssWebstoreApi.Repositories.Abstractions
{
    public interface ICredentialsRepository : IRepository<Credentials>
    {
        Task<Credentials?> GetByAccountIdAsync(ulong id);
        Task<Credentials?> GetByEmailAsync(string email);
    }
}
