using YssWebstoreApi.Api.DTO.Search;

namespace YssWebstoreApi.Api.DTO.Projects
{
    public class ProjectSearchRequest : SearchRequest
    {
        public string? Account { get; set; }
        public string[]? Tags { get; set; }
        public bool PinnedOnly { get; set; }
    }
}
