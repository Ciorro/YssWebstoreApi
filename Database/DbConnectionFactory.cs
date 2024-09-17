using MySql.Data.MySqlClient;
using System.Data;

namespace YssWebstoreApi.Database
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;

        public DbConnectionFactory(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public IDbConnection Create()
        {
            return new MySqlConnection(_connectionString);
        }
    }
}
