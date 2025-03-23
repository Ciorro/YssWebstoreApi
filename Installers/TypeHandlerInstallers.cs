using Dapper;
using YssWebstoreApi.Database.TypeHandlers;

namespace YssWebstoreApi.Installers
{
    public static class TypeHandlerInstallers
    {
        public static void AddTagHandlers(this IApplicationBuilder builder)
        {
            SqlMapper.AddTypeHandler(new TagHandler());
        }
    }
}
