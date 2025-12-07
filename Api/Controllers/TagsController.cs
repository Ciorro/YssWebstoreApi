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
        public async Task<ValueResult<string[]>> GetAllVerifiedTags([FromQuery] string? group, [FromQuery] string? q)
        {
            ValueResult<string[]> result = await _queryMediator.QueryAsync(
                new GetVerifiedTagsQuery()
                {
                    Group = group,
                    SearchText = q
                });

            return result;
        }

        [HttpGet("{tag}")]
        public async Task<ValueResult<string[]>> GetVerifiedTagTree(string tag)
        {
            ValueResult<string[]> result = await _queryMediator.QueryAsync(
                new GetVerifiedTagTreeQuery(tag));

            return result;
        }
    }
}
