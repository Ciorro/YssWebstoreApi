using MediatR;
using YssWebstoreApi.Models;
using YssWebstoreApi.Repositories.Abstractions;

namespace YssWebstoreApi.Features.Commands.Reviews
{
    public class UpdateReviewCommandHandler : IRequestHandler<UpdateReviewCommand, ulong?>
    {
        private readonly IRepository<Review> _reviews;

        public UpdateReviewCommandHandler(IRepository<Review> reviews)
        {
            _reviews = reviews;
        }

        public async Task<ulong?> Handle(UpdateReviewCommand request, CancellationToken cancellationToken)
        {
            return await _reviews.UpdateAsync(request.ReviewId, new Review
            {
                Rate = request.Rate,
                Content = request.Content
            });
        }
    }
}
