using LiteBus.Queries.Abstractions;
using Microsoft.AspNetCore.Mvc;
using YssWebstoreApi.Api.DTO.Projects;
using YssWebstoreApi.Api.DTO.Search;
using YssWebstoreApi.Entities.Tags;
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

        public ProjectsController(IQueryMediator queryMediator)
        {
            _queryMediator = queryMediator;
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
    }
}
