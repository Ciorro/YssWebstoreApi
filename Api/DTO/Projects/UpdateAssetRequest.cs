using YssWebstoreApi.Entities.Tags;

namespace YssWebstoreApi.Api.DTO.Projects
{
    public class UpdateAssetRequest : UpdateProjectRequest
    {
        public override TagCollection GetTags()
        {
            return new TagCollection()
            {
                new Tag("type", "asset")
            };
        }
    }
}
