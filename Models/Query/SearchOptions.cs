using Microsoft.AspNetCore.Mvc;
using YssWebstoreApi.Models.Api;

namespace YssWebstoreApi.Models.Query
{
    public class SearchOptions
    {
        [FromQuery(Name = "")]
        public SortOptions SortOptions { get; set; } = new();
        
        [FromQuery(Name = "")]
        public PageOptions PageOptions { get; set; } = new();
    }
}
