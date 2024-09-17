using YssWebstoreApi.Models;

namespace YssWebstoreApi.Repositories.Abstractions
{
    public interface IAccountRepository : IRepository<Account>
    {
        Task<Account?> GetByUniqueNameAsync(string uniqueName);
    }
}
