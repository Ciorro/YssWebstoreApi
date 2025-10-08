using LiteBus.Commands.Abstractions;
using YssWebstoreApi.Persistance.Repositories.Interfaces;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Reviews.Commands
{
    public class DeleteReviewCommandHandler
        : ICommandHandler<DeleteReviewCommand, Result>
    {
        private readonly IReviewRepository _reviewRepository;

        public DeleteReviewCommandHandler(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public async Task<Result> HandleAsync(DeleteReviewCommand message, CancellationToken cancellationToken = default)
        {
            var review = await _reviewRepository.GetByProjectAndAccount(
                message.ProjectId, message.AccountId);

            if (review is null)
            {
                return CommonErrors.ResourceNotFound;
            }

            await _reviewRepository.DeleteAsync(review.Id);
            return Result.Ok();
        }
    }
}
