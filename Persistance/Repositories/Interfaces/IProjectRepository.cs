using YssWebstoreApi.Models;

namespace YssWebstoreApi.Persistance.Repositories.Interfaces
{
    public interface IProjectRepository : IRepository<Project>
    {
        Task<Project?> GetBySlugAsync(string slug);
    }
}
