namespace YssWebstoreApi.Persistance.Storage
{
    public class SupabaseStorage : IFileStorage
    {
        private readonly HttpClient _httpClient;

        private readonly string _supabaseUrl;
        private readonly string _supabaseKey;
        private readonly string _supabaseBucket;

        public SupabaseStorage(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();

            _supabaseUrl = configuration.GetValue<string>("Supabase:Url")!;
            _supabaseKey = configuration.GetValue<string>("Supabase:Key")!;
            _supabaseBucket = configuration.GetValue<string>("Supabase:Bucket")!;
        }

        public async Task<byte[]> GetData(string path)
        {
            return await _httpClient.GetByteArrayAsync(GetUrl(path));
        }

        public async Task<Stream> GetStream(string path)
        {
            return await _httpClient.GetStreamAsync(GetUrl(path));
        }

        public async Task Create(string path, byte[] data)
        {
            await Supabase.StatelessClient.Storage(_supabaseUrl, _supabaseKey)
                .From(_supabaseBucket)
                .Upload(data, path);
        }

        public async Task Create(string path, Stream stream)
        {
            using (var reader = new MemoryStream())
            {
                await stream.CopyToAsync(reader);

                await Supabase.StatelessClient.Storage(_supabaseUrl, _supabaseKey)
                    .From(_supabaseBucket)
                    .Upload(reader.ToArray(), path);
            }
        }

        public async Task Delete(string path)
        {
            await Supabase.StatelessClient.Storage(_supabaseUrl, _supabaseKey)
                .From(_supabaseBucket)
                .Remove(path);
        }

        public string GetUrl(string path)
        {
            return Supabase.StatelessClient.Storage(_supabaseUrl, _supabaseKey)
                .From(_supabaseBucket)
                .GetPublicUrl(path);
        }
    }
}
