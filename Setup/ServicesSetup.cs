using YssWebstoreApi.Persistance.Storage;
using YssWebstoreApi.Persistance.Storage.Interfaces;
using YssWebstoreApi.Services;

namespace YssWebstoreApi.Setup
{
    public static class ServicesSetup
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<ISessionService, SessionService>();
            services.AddScoped<IStorage, SupabaseStorage>();
            services.AddScoped<IImageStorage, ImageStorage>();
            services.AddScoped<IPostImageStorage, PostImageStorage>();
            return services;
        }
    }
}
