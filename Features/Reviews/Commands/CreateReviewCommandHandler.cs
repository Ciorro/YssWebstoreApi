using LiteBus.Commands.Abstractions;
using YssWebstoreApi.Entities;
using YssWebstoreApi.Persistance.Repositories.Interfaces;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Reviews.Commands
{
    public class CreateReviewCommandHandler
        : ICommandHandler<CreateReviewCommand, ValueResult<Guid>>
    {
        private readonly IRepository<Review> _reviewRepository;
        private readonly TimeProvider _timeProvider;

        public CreateReviewCommandHandler(IRepository<Review> reviewRepository, TimeProvider timeProvider)
        {
            _reviewRepository = reviewRepository;
            _timeProvider = timeProvider;
        }

        public async Task<ValueResult<Guid>> HandleAsync(CreateReviewCommand message, CancellationToken cancellationToken = default)
        {
            var creationTime = _timeProvider.GetUtcNow().UtcDateTime;

            var review = new Review
            {
                Id = Guid.CreateVersion7(creationTime),
                CreatedAt = creationTime,
                UpdatedAt = creationTime,
                AccountId = message.AccountId,
                ProjectId = message.ProjectId,
                Rate = message.Rate,
                Content = message.Content
            };

            await _reviewRepository.InsertAsync(review);
            return review.Id;
        }
    }
}
