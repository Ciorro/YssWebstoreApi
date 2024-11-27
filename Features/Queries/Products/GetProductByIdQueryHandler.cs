using Dapper;
using MediatR;
using System.Data;
using YssWebstoreApi.Database;
using YssWebstoreApi.Models.DTOs.Accounts;
using YssWebstoreApi.Models.DTOs.Product;

namespace YssWebstoreApi.Features.Queries.Products
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, PublicProduct?>
    {
        private readonly IDbConnection _cn;

        public GetProductByIdQueryHandler(IDbConnection dbConnection)
        {
            _cn = dbConnection;
        }

        public async Task<PublicProduct?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var parameters = new
            {
                Id = request.id
            };

            string sql = @"SELECT products.*,
                                  BIT_OR(packages.TargetOS) AS SupportedOS,
                                  AVG(reviews.Rate) AS Rating,
                                  images.Path AS Gallery,
                                  accounts.*
                           FROM products
                           JOIN accounts ON products.AccountId = accounts.Id
                           JOIN credentials ON credentials.AccountId = accounts.Id
                           LEFT JOIN packages ON packages.ProductId = products.Id
                           LEFT JOIN reviews ON reviews.ProductId = products.Id
                           LEFT JOIN products_images ON products_images.ProductId = products.Id
                           LEFT JOIN images ON images.Id = products_images.ImageId
                           WHERE products.Id = @Id
                           GROUP BY products.Id, images.Id
                           ORDER BY products_images.Order ASC";

            PublicProduct? result = null;

            await _cn.QueryAsync<PublicProduct, string, PublicAccount, PublicProduct>(sql, (product, imagePath, account) =>
            {
                result ??= product;
                result.Account = account;
                if (!string.IsNullOrEmpty(imagePath))
                {
                    result.Images.Add(imagePath);
                }
                return result;

            }, parameters, splitOn: "Id, Gallery, Id");

            return result;
        }
    }
}
