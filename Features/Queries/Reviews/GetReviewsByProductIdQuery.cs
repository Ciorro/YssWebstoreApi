using MediatR;
using YssWebstoreApi.Models.Api;
using YssWebstoreApi.Models.DTOs.Review;
using YssWebstoreApi.Models.Query;

namespace YssWebstoreApi.Features.Queries.Reviews
{
    public class GetReviewsByProductIdQuery(ulong productId) : IRequest<Page<PublicReview>>
    {
        public ulong ProductId { get; } = productId;
        public PageOptions PageOptions { get; set; } = new();
    }
}
