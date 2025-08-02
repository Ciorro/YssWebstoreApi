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

            // Storage services
            services.AddScoped<IStorage, SupabaseStorage>();
            services.AddScoped<IImageStorage, ImageStorage>();
            services.AddScoped<IPostImageStorage, PostImageStorage>();
            services.AddScoped<IAvatarStorage, AvatarStorage>();
            services.AddScoped<IProjectStorage, ProjectStorage>();
            return services;
        }
    }
}
