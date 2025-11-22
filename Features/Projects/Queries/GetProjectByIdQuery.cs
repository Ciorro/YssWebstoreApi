using LiteBus.Queries.Abstractions;
using YssWebstoreApi.Api.DTO.Projects;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Projects.Queries
{
    internal class GetProjectByIdQuery(Guid id) : IQuery<Result<ProjectResponse>>
    {
        public Guid Id { get; } = id;
    }
}
