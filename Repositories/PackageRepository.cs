using Dapper;
using System.Data;
using YssWebstoreApi.Models;
using YssWebstoreApi.Repositories.Abstractions;

namespace YssWebstoreApi.Repositories
{
    public class PackageRepository : IRepository<Package>, IDisposable
    {
        private readonly IDbConnection _cn;

        public PackageRepository(IDbConnection dbConnection)
        {
            _cn = dbConnection;
        }

        public async Task<Package?> GetAsync(ulong id)
        {
            var parameters = new
            {
                Id = id
            };

            string sql = @"SELECT packages.* FROM packages WHERE Id=@Id";
            return await _cn.QuerySingleOrDefaultAsync<Package>(sql, parameters);
        }

        public async Task<ulong?> CreateAsync(Package entity)
        {
            var parameters = new
            {
                ProductId = entity.ProductId,
                Name = entity.Name,
                Version = entity.Version,
                DownloadUrl = entity.DownloadUrl,
                FileSize = entity.FileSize,
                TargetOs = entity.TargetOs
            };

            string sql = @"INSERT INTO packages (ProductId,FileSize,Name,Version,DownloadUrl,TargetOS) 
                           VALUES (@ProductId,@FileSize,@Name,@Version,@DownloadUrl,@TargetOS);
                           SELECT Id FROM packages WHERE Id = LAST_INSERT_ID();";

            return await _cn.QuerySingleOrDefaultAsync<ulong>(sql, parameters);
        }

        public async Task<ulong?> UpdateAsync(Package entity)
        {
            if (!entity.Id.HasValue)
            {
                throw new ArgumentNullException(nameof(Package.Id));
            }

            string sql = @"UPDATE packages
                           SET ProductId = @ProductId,
                               Name = @Name,
                               Version = @Version,
                               DownloadUrl = @DownloadUrl,
                               FileSize = @FileSize,
                               TargetOs = @TargetOs
                           WHERE Id = @Id";

            return await _cn.ExecuteAsync(sql, entity) == 1 ? entity.Id : null;
        }

        public async Task<bool> DeleteAsync(ulong id)
        {
            var parameters = new
            {
                Id = id
            };

            string sql = @"DELETE FROM packages WHERE Id = @Id";
            return await _cn.ExecuteAsync(sql, parameters) == 1;
        }

        public void Dispose()
        {
            _cn.Dispose();
        }
    }
}
