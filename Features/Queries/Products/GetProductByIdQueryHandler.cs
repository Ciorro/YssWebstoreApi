﻿using Dapper;
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
                           LEFT JOIN images ON images.GalleryId = products.GalleryId
                           LEFT JOIN comments ON products.ThreadId = comments.ThreadId
                           LEFT JOIN reviews ON reviews.CommentId = comments.Id
                           WHERE products.Id = @Id
                           GROUP BY products.Id, images.Id";

            PublicProduct? result = null;

            await _cn.QueryAsync<PublicProduct, string, PublicAccount, PublicProduct>(sql, (product, imagePath, account) =>
            {
                result ??= product;
                result.Account = account;
                if (!string.IsNullOrEmpty(imagePath))
                {
                    result.Gallery.Add(imagePath);
                }
                return result;

            }, parameters, splitOn: "Id, Gallery, Id");

            return result;
        }
    }
}
