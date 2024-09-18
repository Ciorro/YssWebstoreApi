using YssWebstoreApi.Models;

namespace YssWebstoreApi.Repositories.Abstractions
{
    public interface IPackageRepository : IRepository<Package>
    {
        Task<IEnumerable<Package>> GetPackagesByProductAsync(uint productId);
    }
}
