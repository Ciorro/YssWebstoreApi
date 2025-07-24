using LiteBus.Commands.Abstractions;
using LiteBus.Queries.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YssWebstoreApi.Api.DTO.Posts;
using YssWebstoreApi.Api.DTO.Search;
using YssWebstoreApi.Extensions;
using YssWebstoreApi.Features.Posts.Commands;
using YssWebstoreApi.Features.Posts.Queries;
using YssWebstoreApi.Features.Search.Queries;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly ICommandMediator _commandMediator;
        private readonly IQueryMediator _queryMediator;

        public PostsController(ICommandMediator commandMediator, IQueryMediator queryMediator)
        {
            _commandMediator = commandMediator;
            _queryMediator = queryMediator;
        }

        [HttpGet]
        public async Task<IActionResult> SearchPosts(SearchPostRequest searchRequest)
        {
            Result<Page<PostResponse>> result = await _queryMediator.QueryAsync(
                new SearchPostsQuery()
                {
                    SearchText = searchRequest.SearchQuery,
                    AccountName = searchRequest.Account,
                    TargetProjectId = searchRequest.Project,
                    PageOptions = searchRequest.PageOptions,
                    SortOptions = searchRequest.SortOptions
                });

            if (result.TryGetValue(out var value))
            {
                return Ok(value);
            }

            return BadRequest();
        }

        [HttpGet("{postId:Guid}")]
        public async Task<IActionResult> GetPost(Guid postId)
        {
            Result<PostResponse> result = await _queryMediator.QueryAsync(
                new GetPostByIdQuery(postId));

            if (result.TryGetValue(out var value))
            {
                return Ok(value);
            }

            return NotFound();
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> CreatePost(CreatePostRequest createPost)
        {
            Result<Guid> result = await _commandMediator.SendAsync(
                new CreatePostCommand(User.GetAccountId(), createPost.Title, createPost.Content)
                {
                    TargetProjectId = createPost.TargetProjectId
                });

            if (result.TryGetValue(out var value))
            {
                return CreatedAtRoute(new { Id = value }, null);
            }

            return BadRequest(result.Error);
        }

        [HttpPost("{postId:Guid}/image"), Authorize]
        public async Task<IActionResult> AttachImage(Guid postId, IFormFile file)
        {
            Result result = await _commandMediator.SendAsync(
                new AttachImageToPostCommand(User.GetAccountId(), postId, file));

            if (result.Success)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpDelete("{postId:Guid}/image"), Authorize]
        public async Task<IActionResult> DetachImage(Guid postId)
        {
            Result result = await _commandMediator.SendAsync(
                new RemoveImageFromPostCommand(User.GetAccountId(), postId));

            if (result.Success)
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}
