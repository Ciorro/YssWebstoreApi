using YssWebstoreApi.Models.Interfaces;

namespace YssWebstoreApi.Models
{
    public class Resource : IResource
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid AccountId { get; set; }
        public required long Size { get; set; }
        public required string Path { get; set; }
    }
}
