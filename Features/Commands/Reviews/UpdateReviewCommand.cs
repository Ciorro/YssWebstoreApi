using MediatR;
using YssWebstoreApi.Models.DTOs.Review;

namespace YssWebstoreApi.Features.Commands.Reviews
{
    public class UpdateReviewCommand : IRequest<ulong?>
    {
        public ulong ReviewId { get; }
        public int Rate { get; }
        public string? Content { get; }

        public UpdateReviewCommand(ulong reviewId, UpdateReview updateReview)
        {
            ReviewId = reviewId;
            Rate = updateReview.Rate;
            Content = updateReview.Content;
        }
    }
}
