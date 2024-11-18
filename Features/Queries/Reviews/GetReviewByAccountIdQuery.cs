using MediatR;
using YssWebstoreApi.Models.DTOs.Review;

namespace YssWebstoreApi.Features.Queries.Reviews
{
    public class GetReviewByAccountIdQuery(ulong productId, ulong accountId) : IRequest<PublicReview?>
    {
        public ulong ProductId { get; } = productId;
        public ulong AccountId { get; } = accountId;
    }
}
