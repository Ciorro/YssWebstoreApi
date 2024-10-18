using MediatR;
using YssWebstoreApi.Models.DTOs.Product;

namespace YssWebstoreApi.Features.Queries.Products
{
    public class GetAllProductsQuery : IRequest<IList<PublicProduct>>;
}
