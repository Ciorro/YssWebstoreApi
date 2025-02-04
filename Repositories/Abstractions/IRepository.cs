using YssWebstoreApi.Models.Abstractions;

namespace YssWebstoreApi.Repositories.Abstractions
{
    public interface IRepository<T> where T : IEntity
    {
        Task<T?> GetAsync(ulong id);
        Task<ulong?> CreateAsync(T entity);
        Task<ulong?> UpdateAsync(T entity);
        Task<bool> DeleteAsync(ulong id);
    }
}
