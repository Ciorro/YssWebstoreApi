using YssWebstoreApi.Entities.Tags;

namespace YssWebstoreApi.Api.DTO.Projects
{
    public class CreateToolRequest : CreateProjectRequest
    {
        public required string ToolType { get; set; }

        public override TagCollection GetTags()
        {
            return new TagCollection()
            {
                new Tag("type", "tool"),
                new Tag("tool", ToolType)
            };
        }
    }
}
