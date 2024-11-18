using MediatR;

namespace YssWebstoreApi.Features.Commands.Reviews
{
    public class DeleteReviewCommand(ulong reviewId) : IRequest<ulong?>
    {
        public ulong ReviewId { get; } = reviewId;
    }
}
