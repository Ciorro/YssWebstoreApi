namespace YssWebstoreApi.Repositories.Abstractions
{
    public interface IRepository<T>
    {
        Task<T?> GetAsync(ulong id);
        Task<ulong?> CreateAsync(T entity);
        Task<ulong?> UpdateAsync(ulong id, T entity);
        Task<ulong?> DeleteAsync(ulong id);
    }
}
