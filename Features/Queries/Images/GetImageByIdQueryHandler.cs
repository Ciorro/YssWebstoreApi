using Dapper;
using MediatR;
using System.Data;
using YssWebstoreApi.Models.DTOs.Accounts;
using YssWebstoreApi.Models.DTOs.Image;

namespace YssWebstoreApi.Features.Queries.Images
{
    public class GetImageByIdQueryHandler : IRequestHandler<GetImageByIdQuery, ImageInfo?>
    {
        private readonly IDbConnection _cn;

        public GetImageByIdQueryHandler(IDbConnection connection)
        {
            _cn = connection;
        }

        public async Task<ImageInfo?> Handle(GetImageByIdQuery request, CancellationToken cancellationToken)
        {
            var parameters = new
            {
                Id = request.ImageId
            };

            string sql = @"SELECT images.*,
                                  accounts.*
                           FROM images
                           JOIN accounts ON images.AccountId = accounts.Id
                           WHERE images.Id = @Id";

            return (await _cn.QueryAsync<ImageInfo, PublicAccount, ImageInfo>(sql, (image, account) =>
            {
                image.Account = account;
                return image;

            }, parameters)).SingleOrDefault();
        }
    }
}
