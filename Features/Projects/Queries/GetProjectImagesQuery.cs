using LiteBus.Queries.Abstractions;
using YssWebstoreApi.Api.DTO.Resources;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Projects.Queries
{
    internal class GetProjectImagesQuery(Guid projectId)
        : IQuery<ValueResult<IList<ResourceResponse>>>
    {
        public Guid ProjectId { get; } = projectId;
    }
}
