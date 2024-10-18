using Dapper;
using MediatR;
using System.Data;
using YssWebstoreApi.Models.DTOs.Accounts;
using YssWebstoreApi.Models.DTOs.Product;

namespace YssWebstoreApi.Features.Queries.Products
{
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, IList<PublicProduct>>
    {
        private readonly IDbConnection _cn;

        public GetAllProductsQueryHandler(IDbConnection dbConnection)
        {
            _cn = dbConnection;
        }

        public async Task<IList<PublicProduct>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            string sql = @"SELECT products.*,
                                  BIT_OR(packages.TargetOS) AS SupportedOS,
                                  AVG(reviews.Rate) AS Rating,
                                  images.Path AS Gallery,
                                  accounts.*
                           FROM products
                           JOIN accounts ON products.AccountId = accounts.Id
                           JOIN credentials ON credentials.AccountId = accounts.Id
                           LEFT JOIN packages ON packages.ProductId = products.Id
                           LEFT JOIN images ON images.GalleryId = products.GalleryId
                           LEFT JOIN comments ON products.ThreadId = comments.ThreadId
                           LEFT JOIN reviews ON reviews.CommentId = comments.Id
                           GROUP BY products.Id, images.Id";

            var results = new Dictionary<ulong, PublicProduct>();

            await _cn.QueryAsync<PublicProduct, string, PublicAccount, PublicProduct>(sql, (product, imagePath, account) =>
            {
                if (!results.TryGetValue(product.Id, out var result))
                {
                    result = product;
                    results.Add(product.Id, result);
                }

                result.Account = account;
                if (!string.IsNullOrEmpty(imagePath))
                {
                    result.Gallery.Add(imagePath);
                }
                return result;

            }, null, splitOn: "Id, Gallery, Id");

            return results.Values.ToList();
        }
    }
}
