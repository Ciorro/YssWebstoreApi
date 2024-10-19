using Dapper;
using MediatR;
using System.Data;
using YssWebstoreApi.Models.DTOs.Package;

namespace YssWebstoreApi.Features.Queries.Packages
{
    public class GetPackagesByProductIdQueryHandler : IRequestHandler<GetPackagesByProductIdQuery, IEnumerable<PublicPackage>>
    {
        private readonly IDbConnection _cn;

        public GetPackagesByProductIdQueryHandler(IDbConnection dbConnection)
        {
            _cn = dbConnection;
        }

        public async Task<IEnumerable<PublicPackage>> Handle(GetPackagesByProductIdQuery request, CancellationToken cancellationToken)
        {
            var parameters = new
            {
                ProductId = request.ProductId
            };

            string sql = @"SELECT packages.*
                           FROM packages
                           WHERE ProductId=@ProductId
                           ORDER BY CreatedAt DESC";

            return await _cn.QueryAsync<PublicPackage>(sql, parameters);
        }
    }
}
