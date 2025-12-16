using Dapper;
using System.Data;

namespace YssWebstoreApi.Persistance.TypeHandlers
{
    public class DateOnlyHandler : SqlMapper.TypeHandler<DateOnly>
    {
        public override DateOnly Parse(object value)
        {
            return DateOnly.FromDateTime(
                DateTime.Parse(value.ToString() ?? "0001-01-01"));
        }

        public override void SetValue(IDbDataParameter parameter, DateOnly value)
        {
            parameter.Value = value.ToDateTime(TimeOnly.MinValue);
        }
    }
}
