using Microsoft.AspNetCore.Mvc;
using YssWebstoreApi.Models.Query;

namespace YssWebstoreApi.Models.DTOs.Product
{
    public class SearchProduct : SearchOptions
    {
        public string? SearchQuery { get; set; }
        public string? AccountName { get; set; }
        public bool PinnedOnly { get; set; }

        [FromQuery(Name = "tag")]
        public IList<Tag> Tags { get; set; } = [];
    }
}
