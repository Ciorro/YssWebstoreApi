using MediatR;
using YssWebstoreApi.Models;
using YssWebstoreApi.Repositories.Abstractions;

namespace YssWebstoreApi.Features.Commands.Reviews
{
    public class DeleteReviewCommandHandler : IRequestHandler<DeleteReviewCommand, ulong?>
    {
        private readonly IRepository<Review> _reviews;

        public DeleteReviewCommandHandler(IRepository<Review> reviews)
        {
            _reviews = reviews;
        }

        public async Task<ulong?> Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
        {
            return await _reviews.DeleteAsync(request.ReviewId);
        }
    }
}
