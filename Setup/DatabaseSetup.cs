using Dapper;
using YssWebstoreApi.Persistance;
using YssWebstoreApi.Persistance.TypeHandlers;

namespace YssWebstoreApi.Setup
{
    public static class DatabaseSetup
    {
        public static void InitDatabase(this IApplicationBuilder builder)
        {
            SqlMapper.AddTypeHandler(new TagHandler());
            SqlMapper.AddTypeHandler(new DateOnlyHandler());

            builder.ApplicationServices.GetRequiredService<IDatabaseInitializer>()
                .Initialize();
        }

    }
}
