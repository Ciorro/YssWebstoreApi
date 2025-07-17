using YssWebstoreApi.Services;

namespace YssWebstoreApi.Setup
{
    public static class ServicesSetup
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<ISessionServiceOld, SessionServiceOld>();
            services.AddScoped<ISessionService, SessionService>();
            return services;
        }
    }
}
