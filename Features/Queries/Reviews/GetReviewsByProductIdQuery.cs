using MediatR;
using YssWebstoreApi.Models.DTOs.Review;

namespace YssWebstoreApi.Features.Queries.Reviews
{
    public class GetReviewsByProductIdQuery(ulong productId) : IRequest<IList<PublicReview>>
    {
        public ulong ProductId { get; } = productId;
    }
}
