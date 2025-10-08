using LiteBus.Commands.Abstractions;
using YssWebstoreApi.Persistance.Repositories.Interfaces;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Reviews.Commands
{
    public class UpdateReviewCommandHandler
        : ICommandHandler<UpdateReviewCommand, Result>
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly TimeProvider _timeProvider;

        public UpdateReviewCommandHandler(IReviewRepository reviewRepository, TimeProvider timeProvider)
        {
            _reviewRepository = reviewRepository;
            _timeProvider = timeProvider;
        }

        public async Task<Result> HandleAsync(UpdateReviewCommand message, CancellationToken cancellationToken = default)
        {
            var review = await _reviewRepository.GetByProjectAndAccount(
                message.ProjectId, message.AccountId);

            if (review is null)
            {
                return CommonErrors.ResourceNotFound;
            }

            review.UpdatedAt = _timeProvider.GetUtcNow().UtcDateTime;
            review.Rate = message.Rate;
            review.Content = message.Content;

            await _reviewRepository.UpdateAsync(review);
            return Result.Ok();
        }
    }
}
