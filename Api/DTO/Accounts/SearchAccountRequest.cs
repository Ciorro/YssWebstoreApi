using YssWebstoreApi.Api.DTO.Search;

namespace YssWebstoreApi.Api.DTO.Accounts
{
    public class SearchAccountRequest : SearchRequest
    {
        public Guid? FollowedBy { get; set; }
        public Guid? Following { get; set; }
    }
}
