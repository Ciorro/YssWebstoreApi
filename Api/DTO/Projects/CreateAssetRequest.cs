using YssWebstoreApi.Entities.Tags;

namespace YssWebstoreApi.Api.DTO.Projects
{
    public class CreateAssetRequest : CreateProjectRequest
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
