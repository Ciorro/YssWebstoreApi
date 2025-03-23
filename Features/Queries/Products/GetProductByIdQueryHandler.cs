using Dapper;
using MediatR;
using System.Data;
using YssWebstoreApi.Models;
using YssWebstoreApi.Models.DTOs.Accounts;
using YssWebstoreApi.Models.DTOs.Product;

namespace YssWebstoreApi.Features.Queries.Products
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, DetailedProduct?>
    {
        private readonly IDbConnection _cn;

        public GetProductByIdQueryHandler(IDbConnection dbConnection)
        {
            _cn = dbConnection;
        }

        public async Task<DetailedProduct?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var parameters = new
            {
                Id = request.id
            };

            string sql = @"SELECT
	                           products.Id,
	                           products.CreatedAt,
	                           products.UpdatedAt,
	                           products.AccountId,
	                           products.Name,
	                           products.Description,
	                           products.SourceUrl,
	                           products.IsPinned,
	                           images.Path AS Images,
	                           tags.Tag AS Tags,
	                           accounts.Id,
	                           accounts.UniqueName,
	                           accounts.DisplayName
                           FROM
	                           products
	                           INNER JOIN accounts ON accounts.Id = products.AccountId
	                           LEFT JOIN products_images ON products_images.ProductId = products.Id
	                           LEFT JOIN images ON images.Id = products_images.ImageId
	                           LEFT JOIN products_tags ON products_tags.ProductId = products.Id
	                           LEFT JOIN tags ON tags.Id = products_tags.TagId
                           WHERE
	                           products.Id = @Id
                           ORDER BY
	                           products_images.Order ASC";

            DetailedProduct? result = null;

            await _cn.QueryAsync<DetailedProduct, string, Tag, PublicAccount, DetailedProduct>(sql, (product, imagePath, tag, account) =>
            {
                result ??= product;
                result.Account = account;

                if (!string.IsNullOrEmpty(imagePath))
                {
                    result.Images.Add(imagePath);
                }

                if (tag is not null)
                {
                    result.Tags.Add(tag);
                }

                return result;

            }, parameters, splitOn: "Id, Images, Tags, Id");

            return result;
        }
    }
}
