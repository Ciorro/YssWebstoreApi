using YssWebstoreApi.Entities.Tags;

namespace YssWebstoreApi.Api.DTO.Projects
{
    public class UpdateToolRequest : UpdateProjectRequest
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
