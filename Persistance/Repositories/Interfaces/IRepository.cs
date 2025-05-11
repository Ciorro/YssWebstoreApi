using YssWebstoreApi.Models.Interfaces;

namespace YssWebstoreApi.Persistance.Repositories.Interfaces
{
    public interface IRepository<T> where T : IEntity
    {
        Task<T?> GetAsync(Guid id);
        Task InsertAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(Guid id);
    }
}
