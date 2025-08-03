using YssWebstoreApi.Persistance.Storage.Interfaces;

namespace YssWebstoreApi.Persistance.Storage
{
    public class SupabaseStorage : IStorage
    {
        private readonly string _supabaseUrl;
        private readonly string _supabaseKey;

        public SupabaseStorage(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _supabaseUrl = configuration.GetValue<string>("Supabase:Url")!;
            _supabaseKey = configuration.GetValue<string>("Supabase:Key")!;
        }

        public async Task<string?> GetPublicUrl(string bucketId, string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return null;
            }

            var bucket = await Supabase.StatelessClient.Storage(_supabaseUrl, _supabaseKey)
                .GetBucket(bucketId);

            if (bucket?.Public == true)
            {
                return Supabase.StatelessClient.Storage(_supabaseUrl, _supabaseKey)
                    .From(bucketId)
                    .GetPublicUrl(path);
            }

            return null;
        }

        public async Task<string?> GetPrivateUrl(string bucketId, string path, TimeSpan expiresIn)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return null;
            }

            return await Supabase.StatelessClient.Storage(_supabaseUrl, _supabaseKey)
                .From(bucketId)
                .CreateSignedUrl(path, (int)expiresIn.TotalSeconds);
        }

        public async Task<string?> Upload(string bucketId, string path, Stream stream)
        {
            using (var reader = new MemoryStream())
            {
                await stream.CopyToAsync(reader);

                await Supabase.StatelessClient.Storage(_supabaseUrl, _supabaseKey)
                    .From(bucketId)
                    .Upload(reader.ToArray(), path.Replace('\\', '/'), options: new Supabase.Storage.FileOptions()
                    {
                        Upsert = true
                    });

                return await GetPublicUrl(bucketId, path);
            }
        }

        public async Task Delete(string bucketId, string path)
        {
            await Supabase.StatelessClient.Storage(_supabaseUrl, _supabaseKey)
                .From(bucketId)
                .Remove(path.Replace('\\', '/'));
        }
    }
}
