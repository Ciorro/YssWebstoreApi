using LiteBus.Queries.Abstractions;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Tags.Queries
{
    public class GetVerifiedTagsQuery : IQuery<ValueResult<string[]>>
    {
        public string? Group { get; set; }
        public string? SearchText { get; set; }
    }
}
