using LiteBus.Commands.Abstractions;
using YssWebstoreApi.Persistance.Repositories.Interfaces;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Projects.Commands
{
    public class DeleteProjectReviewCommandHandler
        : ICommandHandler<DeleteProjectReviewCommand, Result>
    {
        private readonly IReviewRepository _reviewRepository;

        public DeleteProjectReviewCommandHandler(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public async Task<Result> HandleAsync(DeleteProjectReviewCommand message, CancellationToken cancellationToken = default)
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
