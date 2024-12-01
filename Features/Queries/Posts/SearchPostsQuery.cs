using MediatR;
using YssWebstoreApi.Models.Api;
using YssWebstoreApi.Models.DTOs.Post;
using YssWebstoreApi.Models.Query;

namespace YssWebstoreApi.Features.Queries.Posts
{
    public class SearchPostsQuery : IRequest<Page<PublicPost>>
    {
        public ulong? ProductId { get; set; }
        public string? AccountName { get; set; }
        public string? SearchQuery { get; set; }
        public int? ContentLength { get; set; }
        public SortOptions SortOptions { get; set; } = new();
        public PageOptions PageOptions { get; set; } = new();

        public SearchPostsQuery() { }

        public SearchPostsQuery(SearchPost searchPostDTO)
        {
            ProductId = searchPostDTO.ProductId;
            AccountName = searchPostDTO.AccountName;
            SearchQuery = searchPostDTO.SearchQuery;
            ContentLength = searchPostDTO.ContentLength;
            SortOptions = searchPostDTO.SortOptions;
            PageOptions = searchPostDTO.PageOptions;
        }
    }
}
