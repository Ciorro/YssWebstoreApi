using YssWebstoreApi.Entities;

namespace YssWebstoreApi.Persistance.Repositories.Interfaces
{
    public interface IReviewRepository : IRepository<Review>
    {
        Task<Review?> GetByProjectAndAccount(Guid projectId, Guid accountId);
    }
}
