using Dapper;
using MediatR;
using System.Data;
using System.Data.SqlClient;
using YssWebstoreApi.Models.DTOs.Accounts;
using YssWebstoreApi.Models.DTOs.Product;
using YssWebstoreApi.Models.Query;

namespace YssWebstoreApi.Features.Queries.Products
{
    public class SearchProductsQueryHandler : IRequestHandler<SearchProductsQuery, Page<PublicProduct>>
    {
        private readonly IDbConnection _cn;

        public SearchProductsQueryHandler(IDbConnection dbConnection)
        {
            _cn = dbConnection;
        }

        public async Task<Page<PublicProduct>> Handle(SearchProductsQuery request, CancellationToken cancellationToken)
        {
            var searchParams = request.SearchParams;
            var sortOptions = request.SortOptions;
            var pageOptions = request.PageOptions;

            var builder = new SqlBuilder();
            var template = builder.AddTemplate(
                $@"SELECT products.*,
                          BIT_OR(packages.TargetOS) AS SupportedOS,
                          AVG(reviews.Rate) AS Rating,
                          images.Path AS Gallery,
                          accounts.*
                   FROM products
                   JOIN accounts ON products.AccountId = accounts.Id
                   JOIN credentials ON credentials.AccountId = accounts.Id
                   LEFT JOIN packages ON packages.ProductId = products.Id
                   LEFT JOIN images ON images.GalleryId = products.GalleryId
                   LEFT JOIN reviews ON reviews.ProductId = products.Id
                   /**where**/
                   GROUP BY products.Id, images.Id
                   /**orderby**/");

            BuildSearchParams(searchParams, builder);
            BuildSortOptions(sortOptions, builder);

            var results = new Dictionary<ulong, PublicProduct>();

            await _cn.QueryAsync<PublicProduct, string, PublicAccount, PublicProduct>(template.RawSql, (product, imagePath, account) =>
            {
                if (!results.TryGetValue(product.Id, out var result))
                {
                    result = product;
                    results.Add(product.Id, result);
                }

                result.Account = account;
                if (!string.IsNullOrEmpty(imagePath))
                {   
                    result.Images.Add(imagePath);
                }
                return result;

            }, template.Parameters, splitOn: "Id, Gallery, Id");

            int pageSize = pageOptions.PageSize ?? results.Count;

            return new Page<PublicProduct>
            {
                PageNumber = pageOptions.PageNumber,
                PageSize = pageSize,
                ItemCount = results.Count,
                Items = results.Values
                    .Skip(pageOptions.PageNumber * pageSize)
                    .Take(pageSize)
                    .ToArray()
            };
        }

        private void BuildSearchParams(SearchProduct searchParams, SqlBuilder builder)
        {
            if (!string.IsNullOrWhiteSpace(searchParams.SearchQuery))
            {
                builder.Where("products.Name LIKE @SearchQuery", new
                {
                    SearchQuery = $"%{searchParams.SearchQuery}%"
                });
            }

            if (!string.IsNullOrWhiteSpace(searchParams.AccountName))
            {
                builder.Where("accounts.UniqueName LIKE @UniqueName", new
                {
                    UniqueName = searchParams.AccountName
                });
            }

            for (int i = 0; i < searchParams.Tags.Count; i++)
            {
                var tag = searchParams.Tags[i];
                var p = new DynamicParameters();
                p.Add($"Tag{i}", $"%{tag}%");

                builder.Where($"products.Tags LIKE @Tag{i}", p);
            }
        }

        private void BuildSortOptions(SortOptions sortOptions, SqlBuilder builder)
        {
            var direction = sortOptions.Descending ? "DESC" : "ASC";

            switch (sortOptions.OrderBy?.ToLower())
            {
                case "updatedat":
                    builder.OrderBy($"products.UpdatedAt {direction}");
                    break;
                case "rating":
                    builder.OrderBy($"Rating {direction}");
                    break;
            }
        }
    }
}
