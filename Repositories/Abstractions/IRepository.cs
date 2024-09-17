namespace YssWebstoreApi.Repositories.Abstractions
{
    public interface IRepository<T>
    {
        Task<T?> GetAsync(uint id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<bool> CreateAsync(T entity);
        Task<bool> UpdateAsync(uint id, T entity);
        Task<bool> DeleteAsync(uint id);
    }
}
