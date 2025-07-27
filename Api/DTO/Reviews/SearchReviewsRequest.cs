using YssWebstoreApi.Api.DTO.Search;

namespace YssWebstoreApi.Api.DTO.Reviews
{
    public class SearchReviewsRequest : SearchRequest
    {
        public Guid? AccountId { get; set; }
    }
}
