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
        public async Task<ValueResult<Page<PostResponse>>> SearchPosts(SearchPostRequest searchRequest)
        {
            ValueResult<Page<PostResponse>> result = await _queryMediator.QueryAsync(
                new SearchPostsQuery()
                {
                    SearchText = searchRequest.SearchQuery,
                    AccountName = searchRequest.Account,
                    TargetProjectId = searchRequest.Project,
                    PageOptions = searchRequest.PageOptions,
                    SortOptions = searchRequest.SortOptions
                });

            return result;
        }

        [HttpGet("{postId:Guid}")]
        public async Task<ValueResult<PostResponse>> GetPost(Guid postId)
        {
            ValueResult<PostResponse> result = await _queryMediator.QueryAsync(
                new GetPostByIdQuery(postId));

            return result;
        }

        [HttpPost, Authorize]
        public async Task<ValueResult<Guid>> CreatePost(CreatePostRequest createPost)
        {
            ValueResult<Guid> result = await _commandMediator.SendAsync(
                new CreatePostCommand(User.GetAccountId(), createPost.Title, createPost.Content)
                {
                    TargetProjectId = createPost.TargetProjectId
                });

            return result;
        }

        [HttpPut("{postId:Guid}"), Authorize]
        public async Task<Result> UpdatePost(Guid postId, UpdatePostRequest updatePost)
        {
            Result result = await _commandMediator.SendAsync(
                new UpdatePostCommand(User.GetAccountId(), postId, updatePost.Title, updatePost.Content)
                {
                    TargetProjectId = updatePost.TargetProjectId
                });

            return result;
        }

        [HttpDelete("{postId:Guid}"), Authorize]
        public async Task<Result> DeletePost(Guid postId)
        {
            Result result = await _commandMediator.SendAsync(
                new DeletePostCommand(User.GetAccountId(), postId));

            return result;
        }

        [HttpPost("{postId:Guid}/image"), Authorize]
        public async Task<ValueResult<string>> UploadCoverImage(Guid postId, IFormFile file)
        {
            ValueResult<string> result = await _commandMediator.SendAsync(
                new UploadCoverImageCommand(User.GetAccountId(), postId, file));

            return result;
        }

        [HttpDelete("{postId:Guid}/image"), Authorize]
        public async Task<Result> DeleteCoverImage(Guid postId)
        {
            Result result = await _commandMediator.SendAsync(
                new RemoveImageFromPostCommand(User.GetAccountId(), postId));

            return result;
        }
    }
}
