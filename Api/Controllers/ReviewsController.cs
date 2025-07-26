using LiteBus.Commands.Abstractions;
using LiteBus.Queries.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YssWebstoreApi.Api.DTO.Reviews;
using YssWebstoreApi.Api.DTO.Search;
using YssWebstoreApi.Extensions;
using YssWebstoreApi.Features.Projects.Commands;
using YssWebstoreApi.Features.Projects.Queries;
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
        public async Task<IActionResult> GetReviews(Guid projectId)
        {
            return Ok(new Page<int>(1, 10, 0, []));
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
                new CreateProjectReviewCommand(User.GetAccountId(), projectId, request.Rate)
                {
                    Content = request.Content
                });

            if (result.TryGetValue(out var value))
            {
                return Ok(value);
            }

            return BadRequest();
        }
    }
}
