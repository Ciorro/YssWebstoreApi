using MediatR;
using YssWebstoreApi.Models.DTOs.Package;

namespace YssWebstoreApi.Features.Queries.Packages
{
    public class GetPackagesByProductIdQuery(ulong productId) : IRequest<IEnumerable<PublicPackage>>
    {
        public ulong ProductId { get; } = productId;
    }
}
