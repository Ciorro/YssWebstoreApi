using YssWebstoreApi.Api.DTO.Search;

namespace YssWebstoreApi.Api.DTO.Posts
{
    public class SearchPostRequest : SearchRequest
    {
        public string? Account { get; set; }
        public Guid? Project { get; set; }
    }
}
