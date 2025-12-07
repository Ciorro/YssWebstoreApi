using LiteBus.Queries.Abstractions;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Tags.Queries
{
    public class GetVerifiedTagTreeQuery(string rootTag) : IQuery<ValueResult<string[]>>
    {
        public string RootTag { get; } = rootTag;
    }
}
