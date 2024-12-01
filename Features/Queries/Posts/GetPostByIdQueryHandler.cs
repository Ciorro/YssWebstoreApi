using Dapper;
using MediatR;
using System.Data;
using YssWebstoreApi.Models.DTOs.Accounts;
using YssWebstoreApi.Models.DTOs.Post;

namespace YssWebstoreApi.Features.Queries.Posts
{
    public class GetPostByIdQueryHandler : IRequestHandler<GetPostByIdQuery, PublicPost?>
    {
        private readonly IDbConnection _cn;

        public GetPostByIdQueryHandler(IDbConnection dbConnection)
        {
            _cn = dbConnection;
        }

        public async Task<PublicPost?> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
        {
            var parameters = new
            {
                Id = request.PostId
            };

            string sql = @"SELECT posts.*,
                                  images.Path AS CoverUrl,
                                  accounts.*
                           FROM posts
                           JOIN accounts ON posts.AccountId = accounts.Id
                           LEFT JOIN images ON posts.ImageId = images.Id
                           WHERE posts.Id = @Id";

            return (await _cn.QueryAsync<PublicPost, PublicAccount, PublicPost>(sql, (post, account) =>
            {
                post.Account = account;
                return post;

            }, parameters)).SingleOrDefault();
        }
    }
}
