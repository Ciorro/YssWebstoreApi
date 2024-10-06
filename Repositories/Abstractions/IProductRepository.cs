using YssWebstoreApi.Models;
using YssWebstoreApi.Models.Api;
using YssWebstoreApi.Models.Query;

namespace YssWebstoreApi.Repositories.Abstractions
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> Search(SearchParams searchParams, SortParams sortParams, Pagination pagination);
    }
}
