using System.Data;

namespace YssWebstoreApi.Database
{
    public interface IDbConnectionFactory
    {
        IDbConnection Create();
    }
}
