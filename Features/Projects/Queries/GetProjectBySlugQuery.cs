using LiteBus.Queries.Abstractions;
using YssWebstoreApi.Api.DTO.Projects;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Projects.Queries
{
    public class GetProjectBySlugQuery(string slug) : IQuery<ValueResult<ProjectResponse>>
    {
        public string Slug { get; } = slug;
    }
}
