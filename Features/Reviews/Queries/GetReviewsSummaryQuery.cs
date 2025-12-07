using LiteBus.Queries.Abstractions;
using YssWebstoreApi.Api.DTO.Reviews;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Reviews.Queries
{
    public class GetReviewsSummaryQuery(Guid projectId) : IQuery<ValueResult<ReviewsSummaryResponse>>
    {
        public Guid ProjectId { get; } = projectId;
    }
}
