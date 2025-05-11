using YssWebstoreApi.Models;
using YssWebstoreApi.Persistance.Repositories;
using YssWebstoreApi.Persistance.Repositories.Interfaces;

namespace YssWebstoreApi.Setup
{
    public static class RepositorySetup
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IRepository<Account>, AccountRepository>();
            return services;
        }
    }
}
