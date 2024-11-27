using static Dapper.SqlMapper;

namespace YssWebstoreApi.Models.Abstractions
{
    public interface IEntity
    {
        ulong? Id { get; set; }
    }
}
