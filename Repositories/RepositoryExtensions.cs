using YssWebstoreApi.Repositories.Abstractions;

namespace YssWebstoreApi.Repositories
{
    public static class RepositoryExtensions
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IPackageRepository, PackageRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<ICredentialRepository, CredentialRepository>();
        }
    }
}
