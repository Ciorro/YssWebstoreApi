using MediatR;

namespace YssWebstoreApi.Features.Commands.Reviews
{
    public class DeleteReviewCommand(ulong reviewId) : IRequest<bool>
    {
        public ulong ReviewId { get; } = reviewId;
    }
}
