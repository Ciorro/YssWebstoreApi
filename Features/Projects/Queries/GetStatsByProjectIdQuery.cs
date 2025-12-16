using LiteBus.Queries.Abstractions;
using YssWebstoreApi.Api.DTO.Projects;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Projects.Queries
{
    public class GetStatsByProjectIdQuery : IQuery<ValueResult<StatisticsResponse>>
    {
        public Guid ProjectId { get; }
        public DateOnly RangeStart { get; set; } = DateOnly.MinValue;
        public DateOnly RangeEnd { get; set; } = DateOnly.MaxValue;

        public GetStatsByProjectIdQuery(Guid projectId)
        {
            ProjectId = projectId;
        }
    }
}
