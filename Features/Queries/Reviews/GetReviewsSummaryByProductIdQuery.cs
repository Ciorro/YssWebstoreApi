using MediatR;
using YssWebstoreApi.Models.DTOs.Review;

namespace YssWebstoreApi.Features.Queries.Reviews
{
    public class GetReviewsSummaryByProductIdQuery(ulong productId) : IRequest<ReviewsSummary?>
    {
        public ulong ProductId { get; } = productId;
    }
}
