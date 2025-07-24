using YssWebstoreApi.Persistance.Storage.Interfaces;

namespace YssWebstoreApi.Persistance.Storage
{
    public class SupabaseStorage : IStorage
    {
        private readonly string _supabaseUrl;
        private readonly string _supabaseKey;
        private readonly string _supabaseBucket;

        public SupabaseStorage(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _supabaseUrl = configuration.GetValue<string>("Supabase:Url")!;
            _supabaseKey = configuration.GetValue<string>("Supabase:Key")!;
            _supabaseBucket = configuration.GetValue<string>("Supabase:Bucket")!;
        }

        public async Task Upload(string path, Stream stream)
        {
            using (var reader = new MemoryStream())
            {
                await stream.CopyToAsync(reader);

                await Supabase.StatelessClient.Storage(_supabaseUrl, _supabaseKey)
                    .From(_supabaseBucket)
                    .Upload(reader.ToArray(), path, options: new Supabase.Storage.FileOptions()
                    {
                        Upsert = true
                    });
            }
        }

        public async Task Delete(string path)
        {
            await Supabase.StatelessClient.Storage(_supabaseUrl, _supabaseKey)
                .From(_supabaseBucket)
                .Remove(path);
        }
    }
}
