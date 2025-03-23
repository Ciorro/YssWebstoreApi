using Dapper;
using System.Data;
using YssWebstoreApi.Models;

namespace YssWebstoreApi.Database.TypeHandlers
{
    public class TagHandler : SqlMapper.TypeHandler<Tag>
    {
        public override Tag? Parse(object value)
        {
            return Tag.TryParse(value.ToString(), out var tag) ? tag : null;
        }

        public override void SetValue(IDbDataParameter parameter, Tag? value)
        {
            if (value is not null)
            {
                parameter.Value = $"{value.Group}-{value.Value}";
            }
        }
    }
}
