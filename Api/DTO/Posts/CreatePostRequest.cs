using System.ComponentModel.DataAnnotations;

namespace YssWebstoreApi.Api.DTO.Posts
{
    public class CreatePostRequest
    {
        [Length(3, 80)]
        public required string Title { get; set; }
        public required string Content { get; set; }
        public Guid? TargetProjectId { get; set; }
    }
}
