using LiteBus.Commands.Abstractions;
using LiteBus.Queries.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YssWebstoreApi.Api.DTO.Projects;
using YssWebstoreApi.Api.DTO.Search;
using YssWebstoreApi.Entities.Tags;
using YssWebstoreApi.Extensions;
using YssWebstoreApi.Features.Posts.Commands;
using YssWebstoreApi.Features.Projects.Commands;
using YssWebstoreApi.Features.Projects.Queries;
using YssWebstoreApi.Features.Search.Queries;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IQueryMediator _queryMediator;
        private readonly ICommandMediator _commandMediator;

        public ProjectsController(IQueryMediator queryMediator, ICommandMediator commandMediator)
        {
            _queryMediator = queryMediator;
            _commandMediator = commandMediator;
        }

        [HttpGet]
        public async Task<IActionResult> SearchProjects(ProjectSearchRequest searchRequest)
        {
            Result<Page<ProjectSearchResult>> result = await _queryMediator.QueryAsync(
                new SearchProjectsQuery()
                {
                    SearchText = searchRequest.SearchQuery,
                    AccountName = searchRequest.Account,
                    Tags = new TagCollection(searchRequest.Tags ?? []),
                    PinnedOnly = searchRequest.PinnedOnly,
                    PageOptions = searchRequest.PageOptions,
                    SortOptions = searchRequest.SortOptions
                });

            if (result.TryGetValue(out var value))
            {
                return Ok(value);
            }

            return BadRequest();
        }

        [HttpGet("{slug}")]
        public async Task<IActionResult> GetProjectBySlug(string slug)
        {
            Result<ProjectResponse> result = await _queryMediator.QueryAsync(
                new GetProjectBySlugQuery(slug));

            if (result.TryGetValue(out var value))
            {
                return Ok(value);
            }

            return BadRequest();
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> CreateProject(CreateProjectRequest request)
        {
            Result<Guid> result = await _commandMediator.SendAsync(
                new CreateProjectCommand(User.GetAccountId(), request.Name, request.Description)
                {
                    Tags = new( request.Tags)
                });

            if (result.TryGetValue(out var value))
            {
                return Ok(value);
            }

            return BadRequest();
        }

        [HttpPost("{projectId:Guid}/pin"), Authorize]
        public async Task<IActionResult> PinProject(Guid projectId)
        {
            Result result = await _commandMediator.SendAsync(
                new SetProjectPinnedCommand(User.GetAccountId(), projectId, true));

            if (result.Success)
            {
                return NoContent();
            }

            return BadRequest();
        }

        [HttpDelete("{projectId:Guid}/pin"), Authorize]
        public async Task<IActionResult> UnpinProject(Guid projectId)
        {
            Result result = await _commandMediator.SendAsync(
                new SetProjectPinnedCommand(User.GetAccountId(), projectId, false));

            if (result.Success)
            {
                return NoContent();
            }

            return BadRequest();
        }
    }
}
