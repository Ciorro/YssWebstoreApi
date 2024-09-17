using Microsoft.AspNetCore.Mvc;

namespace YssWebstoreApi.Models.Query
{
    public class SearchParams
    {
        public string? SearchQuery { get; set; }

        [FromQuery(Name = "tag")]
        public string[]? Tags { get; set; }
    }
}
