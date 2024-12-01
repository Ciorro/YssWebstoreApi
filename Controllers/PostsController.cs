using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YssWebstoreApi.Extensions;
using YssWebstoreApi.Features.Commands.Posts;
using YssWebstoreApi.Features.Commands.Products;
using YssWebstoreApi.Features.Queries.Posts;
using YssWebstoreApi.Features.Queries.Products;
using YssWebstoreApi.Models.DTOs.Post;

namespace YssWebstoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PostsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{postId:int}")]
        public async Task<IActionResult> GetSinglePost(ulong postId)
        {
            var post = await _mediator.Send(new GetPostByIdQuery(postId));

            return post is PublicPost ?
                Ok(post) :
                NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> GetPosts([FromQuery] SearchPost searchPostDTO)
        {
            return Ok(await _mediator.Send(new SearchPostsQuery(searchPostDTO)));
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> CreatePost(CreatePost createPostDTO)
        {
            var resultId = await _mediator.Send(new CreatePostCommand(User.GetUserId(), createPostDTO));

            return resultId.HasValue ?
                Ok(resultId.Value) :
                BadRequest();
        }

        [HttpPut("{postId:int}"), Authorize]
        public async Task<IActionResult> UpdatePost(ulong postId, UpdatePost updatePostDTO)
        {
            var post = await _mediator.Send(new GetPostByIdQuery(postId));
            if (post is null)
            {
                return NotFound();
            }

            if (post.Account.Id != User.GetUserId())
            {
                return Unauthorized();
            }

            var resultId = await _mediator.Send(new UpdatePostCommand(postId, updatePostDTO));

            return resultId.HasValue ?
                Ok(resultId) :
                NotFound();
        }

        [HttpDelete("{postId:int}"), Authorize]
        public async Task<IActionResult> DeletePost(ulong postId)
        {
            var post = await _mediator.Send(new GetPostByIdQuery(postId));
            if (post is null)
            {
                return NotFound();
            }

            if (post.Account.Id != User.GetUserId())
            {
                return Unauthorized();
            }

            var resultId = await _mediator.Send(new DeletePostCommand(postId));

            return resultId.HasValue ?
                Ok(resultId) :
                NotFound();
        }
    }
}
