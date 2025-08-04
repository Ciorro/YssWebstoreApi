using LiteBus.Commands.Abstractions;
using LiteBus.Queries.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YssWebstoreApi.Api.DTO.Reviews;
using YssWebstoreApi.Api.DTO.Search;
using YssWebstoreApi.Extensions;
using YssWebstoreApi.Features.Projects.Commands;
using YssWebstoreApi.Features.Projects.Queries;
using YssWebstoreApi.Features.Search.Queries;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Api.Controllers
{
    [Route("api/projects/{projectId:Guid}/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IQueryMediator _queryMediator;
        private readonly ICommandMediator _commandMediator;

        public ReviewsController(IQueryMediator queryMediator, ICommandMediator commandMediator)
        {
            _queryMediator = queryMediator;
            _commandMediator = commandMediator;
        }

        [HttpGet]
        public async Task<IActionResult> SearchReviews(Guid projectId, SearchReviewsRequest request)
        {
            Result<Page<ReviewResponse>> result = await _queryMediator.QueryAsync(
                new SearchReviewsQuery()
                {
                    ProjectId = projectId,
                    AccountId = request.AccountId,
                    SearchText = request.SearchQuery,
                    PageOptions = request.PageOptions,
                    SortOptions = request.SortOptions
                });

            if (result.TryGetValue(out var value))
            {
                return Ok(value);
            }

            return BadRequest();
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetReviewsSummary(Guid projectId)
        {
            Result<ReviewsSummaryResponse> result = await _queryMediator.QueryAsync(
                new GetReviewsSummaryQuery(projectId));

            if (result.TryGetValue(out var value))
            {
                return Ok(value);
            }

            return BadRequest();
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> CreateReview(Guid projectId, CreateReviewRequest request)
        {
            Result<Guid> result = await _commandMediator.SendAsync(
                new CreateReviewCommand(User.GetAccountId(), projectId, request.Rate)
                {
                    Content = request.Content
                });

            if (result.TryGetValue(out var value))
            {
                return Ok(value);
            }

            return BadRequest();
        }

        [HttpPut, Authorize]
        public async Task<IActionResult> UpdateReview(Guid projectId, UpdateReviewRequest request)
        {
            Result result = await _commandMediator.SendAsync(
               new UpdateReviewCommand(User.GetAccountId(), projectId, request.Rate)
               {
                   Content = request.Content
               });

            if (result.Success)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpDelete, Authorize]
        public async Task<IActionResult> DeleteReview(Guid projectId)
        {
            Result result = await _commandMediator.SendAsync(
               new DeleteReviewCommand(User.GetAccountId(), projectId));

            if (result.Success)
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}
