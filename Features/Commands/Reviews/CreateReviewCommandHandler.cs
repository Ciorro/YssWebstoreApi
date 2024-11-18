using MediatR;
using YssWebstoreApi.Models;
using YssWebstoreApi.Repositories.Abstractions;

namespace YssWebstoreApi.Features.Commands.Reviews
{
    public class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand, ulong?>
    {
        private readonly IRepository<Review> _reviews;

        public CreateReviewCommandHandler(IRepository<Review> reviews)
        {
            _reviews = reviews;
        }

        public async Task<ulong?> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
        {
            return await _reviews.CreateAsync(new Review
            {
                AccountId = request.AccountId,
                ProductId = request.ProductId,
                Rate = request.Rate,
                Content = request.Content
            });
        }
    }
}
