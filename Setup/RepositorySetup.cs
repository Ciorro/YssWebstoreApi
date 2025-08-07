using System.Reflection;
using YssWebstoreApi.Persistance.Repositories.Interfaces;

namespace YssWebstoreApi.Setup
{
    public static class RepositorySetup
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var repositoryTypes = assembly.GetTypes()
                .Where(type => !type.IsAbstract && type.GetInterfaces()
                    .Any(iface => iface.IsGenericType && iface.GetGenericTypeDefinition() == typeof(IRepository<>)));

            foreach (var repositoryType in repositoryTypes)
            {
                var interfaceTypes = repositoryType.GetInterfaces()
                    .Where(iface => iface.GetInterfaces().Any(iface =>
                        iface.IsGenericType && iface.GetGenericTypeDefinition() == typeof(IRepository<>)))
                    .Append(repositoryType.GetInterfaces().First(iface =>
                        iface.IsGenericType && iface.GetGenericTypeDefinition() == typeof(IRepository<>)));

                foreach (var interfaceType in interfaceTypes)
                {
                    services.AddScoped(interfaceType, repositoryType);
                }
            }

            return services;
        }
    }
}
