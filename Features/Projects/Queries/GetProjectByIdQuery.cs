using LiteBus.Queries.Abstractions;
using YssWebstoreApi.Api.DTO.Projects;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Projects.Queries
{
    internal class GetProjectByIdQuery(Guid id) : IQuery<ValueResult<ProjectResponse>>
    {
        public Guid Id { get; } = id;
    }
}
