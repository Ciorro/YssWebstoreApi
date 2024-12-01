using Microsoft.AspNetCore.Mvc;

namespace YssWebstoreApi.Models.Api
{
    public class PageOptions
    {
        [FromQuery(Name = "Page")]
        public int PageNumber { get; set; } = 0;
        public int? PageSize { get; set; }
    }
}
