using System.ComponentModel.DataAnnotations;

namespace YssWebstoreApi.Api.DTO.Projects
{
    public class CreateProjectRequest
    {
        [Length(1, 80)]
        public required string Name { get; set; }
        public required string Description { get; set; }
        public IList<string> Tags { get; set; } = [];
    }
}
