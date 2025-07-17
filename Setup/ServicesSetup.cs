using YssWebstoreApi.Services;

namespace YssWebstoreApi.Setup
{
    public static class ServicesSetup
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<ISessionService, SessionService>();
            return services;
        }
    }
}
