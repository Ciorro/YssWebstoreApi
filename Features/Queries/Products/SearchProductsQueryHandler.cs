﻿using Dapper;
using MediatR;
using System.Data;
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
                $@"SELECT products.Id,
		                  products.CreatedAt,
		                  products.UpdatedAt,
		                  products.AccountId,
		                  products.Name,
		                  products.Description,
		                  products.SourceUrl,
		                  products.Tags,
		                  products.IsPinned,
                          BIT_OR(packages.TargetOS) AS SupportedOS,
                          AVG(reviews.Rate) AS Rating,
                          images.Path AS Gallery,
                          accounts.Id,
                          accounts.CreatedAt,
                          accounts.UpdatedAt,
                          accounts.UniqueName,
                          accounts.DisplayName,
                          accounts.Status
                   FROM products
                   JOIN accounts ON products.AccountId = accounts.Id
                   LEFT JOIN packages ON packages.ProductId = products.Id
                   LEFT JOIN reviews ON reviews.ProductId = products.Id
                   LEFT JOIN products_images ON products_images.ProductId = products.Id
                   LEFT JOIN images ON images.Id = products_images.ImageId
                   /**where**/
                   GROUP BY products.Id, images.Id
                   /**orderby**/");

            BuildSearchParams(searchParams, builder);
            BuildSortOptions(sortOptions, builder);
            builder.OrderBy("products_images.Order ASC");

            var results = new Dictionary<ulong, PublicProduct>();

            await _cn.QueryAsync<PublicProduct, string, PublicAccount, PublicProduct>(template.RawSql, (product, imagePath, account) =>
            {
                if (!results.TryGetValue(product.Id, out var result))
                {
                    result = product;
                    results.Add(product.Id, result);
                }

                if (!string.IsNullOrEmpty(imagePath))
                {
                    result.Images.Add(imagePath);
                }

                result.Account = account;
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

            if (searchParams.PinnedOnly)
            {
                builder.Where("products.IsPinned = TRUE");
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
            var direction = sortOptions.Order.ToString();

            switch (sortOptions.OrderBy?.ToLower())
            {
                case "createdat":
                    builder.OrderBy($"products.CreatedAt {direction}");
                    break;
                case "updatedat":
                    builder.OrderBy($"products.UpdatedAt {direction}");
                    break;
                case "rating":
                    builder.OrderBy($"Rating {direction}");
                    break;
                case "name":
                    builder.OrderBy($"products.Name {direction}");
                    break;
            }
        }
    }
}
