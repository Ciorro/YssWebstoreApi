using Microsoft.AspNetCore.Mvc;

namespace YssWebstoreApi.Api.DTO.Search
{
    public class SearchRequest
    {
        [FromQuery(Name = "q")]
        public string? SearchQuery { get; set; }

        [FromQuery(Name = "")]
        public SortOptions SortOptions { get; set; } = new();

        [FromQuery(Name = "")]
        public PageOptions PageOptions { get; set; } = new();
    }
}
