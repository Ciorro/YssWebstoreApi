using YssWebstoreApi.Entities.Tags;

namespace YssWebstoreApi.Api.DTO.Projects
{
    public class CreateToolRequest : CreateProjectRequest
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
