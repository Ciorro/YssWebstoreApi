using LiteBus.Commands.Abstractions;
using LiteBus.Queries.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YssWebstoreApi.Api.DTO.Posts;
using YssWebstoreApi.Extensions;
using YssWebstoreApi.Features;
using YssWebstoreApi.Features.Posts.Commands;
using YssWebstoreApi.Features.Posts.Queries;

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
            if (!User.TryGetUserId(out var accountId))
            {
                return Unauthorized();
            }

            Result<Guid> result = await _commandMediator.SendAsync(
                new CreatePostCommand(accountId, createPost.Title, createPost.Content)
                {
                    ImageResourceId = createPost.ImageResourceId,
                    TargetProjectId = createPost.TargetProjectId
                });

            if (result.TryGetValue(out var value))
            {
                return CreatedAtRoute(new { Id = value }, null);
            }

            return BadRequest(result.Error);
        }
    }
}
