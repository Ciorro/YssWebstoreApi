using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YssWebstoreApi.Extensions;
using YssWebstoreApi.Features.Commands.Reviews;
using YssWebstoreApi.Features.Queries.Reviews;
using YssWebstoreApi.Models.Api;
using YssWebstoreApi.Models.DTOs.Review;

namespace YssWebstoreApi.Controllers.Reviews
{
    [Route("api/products/{productId:int}/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReviewsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetProductReviews(ulong productId, PageOptions pageOptions)
        {
            return Ok(await _mediator.Send(new GetReviewsByProductIdQuery(productId)
            {
                PageOptions = pageOptions
            }));
        }

        [HttpGet("account/{accountId:int}")]
        public async Task<IActionResult> GetReview(ulong productId, ulong accountId)
        {
            var review = await _mediator.Send(new GetReviewByAccountIdQuery(productId, accountId));

            return review is PublicReview ?
                Ok(review) :
                NotFound();
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetReviewsSummary(ulong productId)
        {
            var summary = await _mediator.Send(new GetReviewsSummaryByProductIdQuery(productId));

            return summary is ReviewsSummary ?
                Ok(summary) :
                NotFound();
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> CreateReview(ulong productId, [FromBody] CreateReview createReviewDTO)
        {
            var existingReview = await _mediator.Send(new GetReviewByAccountIdQuery(productId, User.GetUserId()));
            if (existingReview is not null)
            {
                return Conflict();
            }

            var resultId = await _mediator.Send(new CreateReviewCommand(createReviewDTO)
            {
                AccountId = User.GetUserId(),
                ProductId = productId
            });

            return resultId.HasValue ?
                Ok(resultId) :
                BadRequest();
        }

        [HttpPut, Authorize]
        public async Task<IActionResult> UpdateReview(ulong productId, [FromBody] UpdateReview updateReviewDTO)
        {
            var existingReview = await _mediator.Send(new GetReviewByAccountIdQuery(productId, User.GetUserId()));
            if (existingReview is null)
            {
                return NotFound();
            }

            if (existingReview.Account.Id != User.GetUserId())
            {
                return Unauthorized();
            }

            var resultId = await _mediator.Send(new UpdateReviewCommand(existingReview.Id, updateReviewDTO));

            return resultId.HasValue ?
                Ok(resultId) :
                Problem();
        }

        [HttpDelete, Authorize]
        public async Task<IActionResult> DeleteReview(ulong productId)
        {
            var existingReview = await _mediator.Send(new GetReviewByAccountIdQuery(productId, User.GetUserId()));
            if (existingReview is null)
            {
                return NotFound();
            }

            if (existingReview.Account.Id != User.GetUserId())
            {
                return Unauthorized();
            }

            var resultId = await _mediator.Send(new DeleteReviewCommand(existingReview.Id));

            return resultId.HasValue ?
                Ok(resultId) :
                Problem();
        }
    }
}
