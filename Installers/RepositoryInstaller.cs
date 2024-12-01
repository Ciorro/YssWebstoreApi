using YssWebstoreApi.Models;
using YssWebstoreApi.Repositories;
using YssWebstoreApi.Repositories.Abstractions;

namespace YssWebstoreApi.Installers
{
    public static class RepositoryInstaller
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IRepository<Account>, AccountRepository>();
            services.AddScoped<IRepository<Product>, ProductRepository>();
            services.AddScoped<IRepository<Package>, PackageRepository>();
            services.AddScoped<IRepository<Review>, ReviewRepository>();
            services.AddScoped<IAttachmentRepository<Image>, ImageRepository>();
            services.AddScoped<IRepository<Image>>(s => 
                s.GetRequiredService<IAttachmentRepository<Image>>());

            services.AddScoped<ICredentialsRepository, CredentialsRepository>();
            services.AddScoped<IRepository<Credentials>>(s =>
            {
                return s.GetRequiredService<ICredentialsRepository>();
            });
        }
    }
}
