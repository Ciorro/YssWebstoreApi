using YssWebstoreApi.Entities;

namespace YssWebstoreApi.Persistance.Repositories.Interfaces
{
    public interface IAccountRepository : IRepository<Account>
    {
        Task<Account?> GetByEmailAsync(string email);
        Task<Account?> GetByUniqueNameAsync(string uniqueName);
    }
}
