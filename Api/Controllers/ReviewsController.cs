using LiteBus.Commands.Abstractions;
using LiteBus.Queries.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YssWebstoreApi.Api.DTO.Reviews;
using YssWebstoreApi.Api.DTO.Search;
using YssWebstoreApi.Extensions;
using YssWebstoreApi.Features.Reviews.Commands;
using YssWebstoreApi.Features.Reviews.Queries;
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
        public async Task<ValueResult<Page<ReviewResponse>>> SearchReviews(Guid projectId, SearchReviewsRequest request)
        {
            ValueResult<Page<ReviewResponse>> result = await _queryMediator.QueryAsync(
                new SearchReviewsQuery()
                {
                    ProjectId = projectId,
                    AccountId = request.AccountId,
                    SearchText = request.SearchQuery,
                    PageOptions = request.PageOptions,
                    SortOptions = request.SortOptions
                });

            return result;
        }

        [HttpGet("summary")]
        public async Task<ValueResult<ReviewsSummaryResponse>> GetReviewsSummary(Guid projectId)
        {
            ValueResult<ReviewsSummaryResponse> result = await _queryMediator.QueryAsync(
                new GetReviewsSummaryQuery(projectId));

            return result;
        }

        [HttpPost, Authorize]
        public async Task<ValueResult<Guid>> CreateReview(Guid projectId, CreateReviewRequest request)
        {
            ValueResult<Guid> result = await _commandMediator.SendAsync(
                new CreateReviewCommand(User.GetAccountId(), projectId, request.Rate)
                {
                    Content = request.Content
                });

            return result;
        }

        [HttpPut, Authorize]
        public async Task<Result> UpdateReview(Guid projectId, UpdateReviewRequest request)
        {
            Result result = await _commandMediator.SendAsync(
               new UpdateReviewCommand(User.GetAccountId(), projectId, request.Rate)
               {
                   Content = request.Content
               });

            return result;
        }

        [HttpDelete, Authorize]
        public async Task<Result> DeleteReview(Guid projectId)
        {
            Result result = await _commandMediator.SendAsync(
               new DeleteReviewCommand(User.GetAccountId(), projectId));

            return result;
        }
    }
}
