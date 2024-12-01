using YssWebstoreApi.Models.Validation;

namespace YssWebstoreApi.Models.DTOs.Post
{
    public class CreatePost
    {
        public required string Title { get; set; }
        public required string Content { get; set; }
        public ulong? ProductId { get; set; }

        [MaxContentLength(3000000)]
        public IFormFile? Cover { get; set; }
    }
}
