using Dapper;
using LiteBus.Queries.Abstractions;
using System.Data;
using YssWebstoreApi.Api.DTO.Projects;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Projects.Queries
{
    public class GetStatsByProjectIdQueryHandler
        : IQueryHandler<GetStatsByProjectIdQuery, ValueResult<StatisticsResponse>>
    {
        private readonly IDbConnection _db;

        public GetStatsByProjectIdQueryHandler(IDbConnection dbConnection)
        {
            _db = dbConnection;
        }

        public async Task<ValueResult<StatisticsResponse>> HandleAsync(GetStatsByProjectIdQuery message, CancellationToken cancellationToken = default)
        {
            var statistics = await _db.QueryAsync<(DateOnly Date, int Downloads)>(
                """
                SELECT CreatedAt::DATE, COUNT(*) FROM Downloads 
                WHERE ProjectId = @ProjectId
                  AND CreatedAt > @RangeStart
                  AND CreatedAt < @RangeEnd
                GROUP BY CreatedAt::DATE
                """,
                new
                {
                    message.ProjectId,
                    message.RangeStart,
                    message.RangeEnd
                });

            return new StatisticsResponse
            {
                RangeStart = message.RangeStart,
                RangeEnd = message.RangeEnd,
                Downloads = statistics
                    .Select(x => (x.Date, x.Downloads))
                    .ToDictionary()
            };
        }
    }
}
