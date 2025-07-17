namespace YssWebstoreApi.Models.DTOs.Posts
{
    public class CreatePostRequest
    {
        public required string Title { get; set; }
        public required string Content { get; set; }
        public Guid? ImageResourceId { get; set; }
        public Guid? TargetProjectId { get; set; }
    }
}
