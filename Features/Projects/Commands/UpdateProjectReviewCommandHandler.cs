using LiteBus.Commands.Abstractions;
using YssWebstoreApi.Persistance.Repositories.Interfaces;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Projects.Commands
{
    public class UpdateProjectReviewCommandHandler
        : ICommandHandler<UpdateProjectReviewCommand, Result>
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly TimeProvider _timeProvider;

        public UpdateProjectReviewCommandHandler(IReviewRepository reviewRepository, TimeProvider timeProvider)
        {
            _reviewRepository = reviewRepository;
            _timeProvider = timeProvider;
        }

        public async Task<Result> HandleAsync(UpdateProjectReviewCommand message, CancellationToken cancellationToken = default)
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
