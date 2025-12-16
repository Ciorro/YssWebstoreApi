using YssWebstoreApi.Entities.Tags;

namespace YssWebstoreApi.Api.DTO.Projects
{
    public class CreateAssetRequest : CreateProjectRequest
    {
        public required string AssetType { get; set; }
        public required string Style { get; set; }
        public List<string>? Themes { get; set; }

        public override TagCollection GetTags()
        {
            var tags = new TagCollection()
            {
                new Tag("type", "asset"),
                new Tag("asset", AssetType),
                new Tag("style", Style)
            };

            tags.AddValuesToGroup("theme", Themes ?? []);
            return tags;
        }
    }
}
