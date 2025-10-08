using System.ComponentModel.DataAnnotations;
using YssWebstoreApi.Entities.Tags;

namespace YssWebstoreApi.Api.DTO.Projects
{
    public abstract class CreateProjectRequest
    {
        [Length(1, 80)]
        public required string Name { get; set; }
        public required string Description { get; set; }

        public abstract TagCollection GetTags();
    }
}
