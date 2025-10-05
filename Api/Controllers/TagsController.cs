using LiteBus.Queries.Abstractions;
using Microsoft.AspNetCore.Mvc;
using YssWebstoreApi.Features.Tags.Queries;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly IQueryMediator _queryMediator;

        public TagsController(IQueryMediator queryMediator)
        {
            _queryMediator = queryMediator;
        }

        [HttpGet()]
        public async Task<IActionResult> GetAllVerifiedTags([FromQuery] string? group, [FromQuery] string? q)
        {
            Result<string[]> result = await _queryMediator.QueryAsync(
                new GetVerifiedTagsQuery()
                {
                    Group = group,
                    SearchText = q
                });

            if (result.TryGetValue(out var value))
            {
                return Ok(value);
            }

            return BadRequest(result.Error);
        }

        [HttpGet("{tag}")]
        public async Task<IActionResult> GetVerifiedTagTree(string tag)
        {
            Result<string[]> result = await _queryMediator.QueryAsync(
                new GetVerifiedTagTreeQuery(tag));

            if (result.TryGetValue(out var value))
            {
                return Ok(value);
            }

            return BadRequest();
        }
    }
}
