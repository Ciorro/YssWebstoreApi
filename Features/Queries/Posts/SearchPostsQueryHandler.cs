using Dapper;
using MediatR;
using System.Data;
using YssWebstoreApi.Models.DTOs.Accounts;
using YssWebstoreApi.Models.DTOs.Post;
using YssWebstoreApi.Models.Query;

namespace YssWebstoreApi.Features.Queries.Posts
{
    public class SearchPostsQueryHandler : IRequestHandler<SearchPostsQuery, Page<PublicPost>>
    {
        private readonly IDbConnection _cn;

        public SearchPostsQueryHandler(IDbConnection dbConnection)
        {
            _cn = dbConnection;
        }

        public async Task<Page<PublicPost>> Handle(SearchPostsQuery request, CancellationToken cancellationToken)
        {
            var sortOptions = request.SortOptions;
            var pageOptions = request.PageOptions;

            var builder = new SqlBuilder();
            var template = builder.AddTemplate(
                @"SELECT posts.*,
                         images.Path AS CoverUrl,
                         accounts.*
                  FROM posts
                  JOIN accounts ON posts.AccountId = accounts.Id
                  LEFT JOIN images ON posts.ImageId = images.Id
                  /**where**/
                  /**orderby**/");

            BuildSearchParams(request, builder);
            BuildSortOptions(sortOptions, builder);

            var results = (await _cn.QueryAsync<PublicPost, PublicAccount, PublicPost>(template.RawSql, (post, account) =>
            {
                var contentLength = request.ContentLength.HasValue ?
                    Math.Clamp(request.ContentLength.Value, 0, post.Content.Length) :
                    post.Content.Length;

                post.Account = account;
                post.Content = post.Content.Substring(0, contentLength);

                return post;

            }, template.Parameters)).ToList();

            int pageSize = pageOptions.PageSize ?? results.Count;

            return new Page<PublicPost>
            {
                PageNumber = pageOptions.PageNumber,
                PageSize = pageSize,
                ItemCount = results.Count,
                Items = results
                    .Skip(pageOptions.PageNumber * pageSize)
                    .Take(pageSize)
                    .ToArray()
            };
        }

        private static void BuildSearchParams(SearchPostsQuery request, SqlBuilder builder)
        {
            if (request.ProductId.HasValue)
            {
                builder.Where("posts.ProductId = @ProductId", new
                {
                    ProductId = request.ProductId.Value
                });
            }

            if (!string.IsNullOrWhiteSpace(request.AccountName))
            {
                builder.Where("accounts.UniqueName = @AccountName", new
                {
                    AccountName = request.AccountName
                });
            }

            if (!string.IsNullOrWhiteSpace(request.SearchQuery))
            {
                builder.Where("posts.Title LIKE @SearchQuery", new
                {
                    SearchQuery = $"%{request.SearchQuery}%"
                });
            }
        }

        private static void BuildSortOptions(SortOptions sortOptions, SqlBuilder builder)
        {
            var direction = sortOptions.Descending ? "DESC" : "ASC";

            switch (sortOptions.OrderBy?.ToLower())
            {
                case "createdat":
                    builder.OrderBy($"posts.CreatedAt {direction}");
                    break;
                case "updatedat":
                    builder.OrderBy($"posts.UpdatedAt {direction}");
                    break;
            }
        }
    }
}
