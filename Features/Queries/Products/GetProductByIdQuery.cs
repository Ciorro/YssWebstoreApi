using MediatR;
using YssWebstoreApi.Models.DTOs.Product;

namespace YssWebstoreApi.Features.Queries.Products
{
    public record GetProductByIdQuery(ulong id) : IRequest<PublicProduct?>;
}
