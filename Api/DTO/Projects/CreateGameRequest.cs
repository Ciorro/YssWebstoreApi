using YssWebstoreApi.Entities.Tags;

namespace YssWebstoreApi.Api.DTO.Projects
{
    public class CreateGameRequest : CreateProjectRequest
    {
        public required string Genre { get; set; }
        public List<string>? Tags { get; set; }
        public List<string>? Features { get; set; } 

        public override TagCollection GetTags()
        {
            var tags = new TagCollection()
            {
                new Tag("type", "game"),
                new Tag("genre", Genre)
            };

            tags.AddValuesToGroup("tag", Tags ?? []);
            tags.AddValuesToGroup("feature", Features ?? []);

            return tags;
        }
    }
}
