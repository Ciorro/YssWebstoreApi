using Dapper;
using Npgsql;
using YssWebstoreApi.Persistance;
using YssWebstoreApi.Persistance.TypeHandlers;

namespace YssWebstoreApi.Setup
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
