using LiteBus.Queries.Abstractions;
using YssWebstoreApi.Api.DTO.Packages;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Projects.Queries
{
    public class GetProjectPackagesQuery(Guid projectId) : IQuery<Result<IList<PackageResponse>>>
    {
        public Guid ProjectId { get; } = projectId;
    }
}
