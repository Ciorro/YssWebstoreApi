using Microsoft.AspNetCore.Mvc;

namespace YssWebstoreApi.Models.DTOs.Product
{
    public class SearchProduct
    {
        public string? SearchQuery { get; set; }
        public string? AccountName { get; set; }

        [FromQuery(Name = "tag")]
        public IList<Tag> Tags { get; set; } = [];
    }
}
