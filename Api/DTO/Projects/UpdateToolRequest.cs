using YssWebstoreApi.Entities.Tags;

namespace YssWebstoreApi.Api.DTO.Projects
{
    public class UpdateToolRequest : UpdateProjectRequest
    {
        public override TagCollection GetTags()
        {
            return new TagCollection()
            {
                new Tag("type", "tool")
            };
        }
    }
}
