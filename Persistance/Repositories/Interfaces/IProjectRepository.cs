using YssWebstoreApi.Entities;

namespace YssWebstoreApi.Persistance.Repositories.Interfaces
{
    public interface IProjectRepository : IRepository<Project>
    {
        Task<Project?> GetBySlugAsync(string slug);
    }
}
