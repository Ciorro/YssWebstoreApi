using YssWebstoreApi.Models;
using YssWebstoreApi.Repositories;
using YssWebstoreApi.Repositories.Abstractions;

namespace YssWebstoreApi.Installers
{
    public static class RepositoryInstaller
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IRepository<Product>, ProductRepository>();
            services.AddScoped<IRepository<Account>, AccountRepository>();

            services.AddScoped<ICredentialsRepository, CredentialsRepository>();
            services.AddScoped<IRepository<Credentials>>(s =>
            {
                return s.GetRequiredService<ICredentialsRepository>();
            });
        }
    }
}
