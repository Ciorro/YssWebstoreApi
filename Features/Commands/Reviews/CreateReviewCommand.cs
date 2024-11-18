using MediatR;
using System.Diagnostics.CodeAnalysis;
using YssWebstoreApi.Models.DTOs.Review;

namespace YssWebstoreApi.Features.Commands.Reviews
{
    public class CreateReviewCommand : IRequest<ulong?>
    {
        public required ulong AccountId { get; set; }
        public required ulong ProductId { get; set; }
        public int Rate { get; set; }
        public string? Content { get; set; }

        [SetsRequiredMembers]
        public CreateReviewCommand(ulong productId, ulong accountId, CreateReview createReview)
            :this(createReview)
        {
            AccountId = accountId;
            ProductId = productId;
        }

        public CreateReviewCommand(CreateReview createReview)
        {
            Rate = createReview.Rate;
            Content = createReview.Content;
        }
    }
}
