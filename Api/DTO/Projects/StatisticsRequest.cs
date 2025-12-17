using Microsoft.AspNetCore.Mvc;

namespace YssWebstoreApi.Api.DTO.Projects
{
    public class StatisticsRequest
    {
        [FromQuery]
        public DateOnly? RangeStart { get; set; }

        [FromQuery]
        public DateOnly? RangeEnd { get; set; }
    }
}
