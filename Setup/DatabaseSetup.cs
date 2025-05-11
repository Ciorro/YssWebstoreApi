using Dapper;
using YssWebstoreApi.Persistance;
using YssWebstoreApi.Persistance.TypeHandlers;

namespace YssWebstoreApi.Installers
{
    public static class DatabaseSetup
    {
        public static void InitDatabase(this IApplicationBuilder builder)
        {
            SqlMapper.AddTypeHandler(new TagHandler());

            builder.ApplicationServices.GetRequiredService<IDatabaseInitializer>()
                .Initialize();
        }

    }
}
