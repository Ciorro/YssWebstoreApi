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
            var review = await _reviews.GetAsync(request.ReviewId);
            if (review is not null)
            {
                review.Rate = request.Rate;
                review.Content = request.Content;

                return await _reviews.UpdateAsync(review);
            }

            //TODO: return error
            return null;
        }
    }
}
