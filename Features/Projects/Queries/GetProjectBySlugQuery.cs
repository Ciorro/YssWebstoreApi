using LiteBus.Queries.Abstractions;
using YssWebstoreApi.Api.DTO.Projects;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Projects.Queries
{
    public class GetProjectBySlugQuery(string slug) : IQuery<Result<ProjectResponse>>
    {
        public string Slug { get; } = slug;
    }
}
