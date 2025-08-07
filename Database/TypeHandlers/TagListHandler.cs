using Dapper;
using System.Data;
using YssWebstoreApi.Models;

namespace YssWebstoreApi.Database.TypeHandlers
{
    public class TagListHandler : SqlMapper.TypeHandler<IList<Tag>>
    {
        public override IList<Tag>? Parse(object value)
        {
            return new List<Tag>(value.ToString()?.Split().Select(Tag.Parse) ?? []);
        }

        public override void SetValue(IDbDataParameter parameter, IList<Tag>? value)
        {
            parameter.Value = string.Join(' ', value?.Select(x => x.ToString()) ?? []);
        }
    }
}
