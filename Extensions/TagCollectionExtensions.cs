using YssWebstoreApi.Entities.Tags;

namespace YssWebstoreApi.Extensions
{
    public static class TagCollectionExtensions
    {
        public static List<string> ToStringList(this TagCollection tags)
        {
            return tags.Select(tag => tag.ToString()).ToList();
        }
    }
}
