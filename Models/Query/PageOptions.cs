using Microsoft.AspNetCore.Mvc;

namespace YssWebstoreApi.Models.Query
{
    public class PageOptions
    {
        [FromQuery(Name = "Page")]
        public int PageNumber { get; set; }
        public int? PageSize { get; set; }
    }
}
