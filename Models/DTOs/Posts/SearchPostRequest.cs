using YssWebstoreApi.Models.DTOs.Search;

namespace YssWebstoreApi.Models.DTOs.Posts
{
    public class SearchPostRequest : SearchRequest
    {
        public string? Account { get; set; }
        public string? Project { get; set; }
    }
}
