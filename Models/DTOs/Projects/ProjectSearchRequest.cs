using YssWebstoreApi.Models.DTOs.Search;

namespace YssWebstoreApi.Models.DTOs.Projects
{
    public class ProjectSearchRequest : SearchRequest
    {
        public string? Account { get; set; }
        public string[]? Tags { get; set; }
        public bool PinnedOnly { get; set; }
    }
}
