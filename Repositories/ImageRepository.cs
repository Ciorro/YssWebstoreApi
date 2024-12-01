using Dapper;
using System.Data;
using YssWebstoreApi.Models;
using YssWebstoreApi.Repositories.Abstractions;
using YssWebstoreApi.Services.Files;

namespace YssWebstoreApi.Repositories
{
    public class ImageRepository : IAttachmentRepository<Image>, IDisposable
    {
        private readonly IDbConnection _cn;
        private readonly IFilesystemService _filesystem;

        public ImageRepository(IDbConnection dbConnection, IFilesystemService filesystem)
        {
            _cn = dbConnection;
            _filesystem = filesystem;
        }

        public async Task<Image?> GetAsync(ulong id)
        {
            var parameters = new
            {
                Id = id
            };

            string sql = @"SELECT images.* FROM images WHERE Id=@Id";
            return await _cn.QuerySingleOrDefaultAsync<Image>(sql, parameters);
        }

        public async Task<ulong?> CreateAsync(Image entity)
        {
            var parameters = new
            {
                Title = entity.Title,
                Path = entity.Path,
                AccountId = entity.AccountId
            };

            string sql = @"INSERT INTO images (Title,Path,AccountId) 
                           VALUES (@Title,@Path,@AccountId) RETURNING Id";

            return await _cn.QuerySingleOrDefaultAsync<ulong>(sql, parameters);
        }

        public async Task<ulong?> UpdateAsync(Image entity)
        {
            if (!entity.Id.HasValue)
            {
                throw new ArgumentNullException(nameof(Image.Id));
            }

            string sql = @"UPDATE images
                           SET Title = @Title,
                               Path = @Path,
                               AccountId = @AccountId
                           WHERE Id = @Id";

            return await _cn.ExecuteAsync(sql, entity) == 1 ? entity.Id : null;
        }

        public async Task<ulong?> DeleteAsync(ulong id)
        {
            var parameters = new
            {
                Id = id
            };

            string sql = @"DELETE FROM images WHERE Id = @Id RETURNING Id";
            return await _cn.QuerySingleOrDefaultAsync<ulong>(sql, parameters);
        }

        public void Dispose()
        {
            _cn.Dispose();
        }

        public async Task<ulong?> CreateAndAttachAsync(Image entity, Stream stream)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(entity.Path, nameof(Image.Path));

            using (var fs = _filesystem.OpenWrite(entity.Path))
            {
                await stream.CopyToAsync(fs);
            }
            return await CreateAsync(entity);
        }

        public async Task<ulong?> DeleteAndDetachAsync(ulong id)
        {
            var image = await GetAsync(id);

            if (image is not null)
            {
                if (await DeleteAsync(id) == id)
                {
                    if (await _filesystem.Exists(image.Path!))
                    {
                        await _filesystem.Delete(image.Path!);
                    }
                }
            }

            return image?.Id;
        }
    }
}
