using LiteBus.Commands.Abstractions;
using LiteBus.Queries.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YssWebstoreApi.Api.DTO.Posts;
using YssWebstoreApi.Api.DTO.Search;
using YssWebstoreApi.Extensions;
using YssWebstoreApi.Features.Posts.Commands;
using YssWebstoreApi.Features.Posts.Queries;
using YssWebstoreApi.Features.Projects.Commands;
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
                return Ok(value);
            }

            return BadRequest(result.Error);
        }

        [HttpPut("{postId:Guid}"), Authorize]
        public async Task<IActionResult> UpdatePost(Guid postId, UpdatePostRequest updatePost)
        {
            Result result = await _commandMediator.SendAsync(
                new UpdatePostCommand(User.GetAccountId(), postId, updatePost.Title, updatePost.Content)
                {
                    TargetProjectId = updatePost.TargetProjectId
                });

            if (result.Success)
            {
                return NoContent();
            }

            return BadRequest();
        }

        [HttpDelete("{postId:Guid}"), Authorize]
        public async Task<IActionResult> DeletePost(Guid postId)
        {
            Result result = await _commandMediator.SendAsync(
                new DeletePostCommand(User.GetAccountId(), postId));

            if (result.Success)
            {
                return NoContent();
            }

            return BadRequest();
        }

        [HttpPost("{postId:Guid}/image"), Authorize]
        public async Task<IActionResult> UploadCoverImage(Guid postId, IFormFile file)
        {
            Result<string> result = await _commandMediator.SendAsync(
                new UploadCoverImageCommand(User.GetAccountId(), postId, file));

            if (result.TryGetValue(out var value))
            {
                return Ok(value);
            }

            return BadRequest();
        }

        [HttpDelete("{postId:Guid}/image"), Authorize]
        public async Task<IActionResult> DeleteCoverImage(Guid postId)
        {
            Result result = await _commandMediator.SendAsync(
                new RemoveImageFromPostCommand(User.GetAccountId(), postId));

            if (result.Success)
            {
                return NoContent();
            }

            return BadRequest();
        }
    }
}
