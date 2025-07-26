using LiteBus.Queries.Abstractions;
using YssWebstoreApi.Api.DTO.Reviews;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Projects.Queries
{
    public class GetReviewsSummaryQuery(Guid projectId) : IQuery<Result<ReviewsSummaryResponse>>
    {
        public Guid ProjectId { get; } = projectId;
    }
}
