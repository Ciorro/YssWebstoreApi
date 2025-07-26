using LiteBus.Commands.Abstractions;
using YssWebstoreApi.Entities;
using YssWebstoreApi.Persistance.Repositories.Interfaces;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Projects.Commands
{
    public class CreateProjectReviewCommandHandler
        : ICommandHandler<CreateProjectReviewCommand, Result<Guid>>
    {
        private readonly IRepository<Review> _reviewRepository;
        private readonly TimeProvider _timeProvider;

        public CreateProjectReviewCommandHandler(IRepository<Review> reviewRepository, TimeProvider timeProvider)
        {
            _reviewRepository = reviewRepository;
            _timeProvider = timeProvider;
        }

        public async Task<Result<Guid>> HandleAsync(CreateProjectReviewCommand message, CancellationToken cancellationToken = default)
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
