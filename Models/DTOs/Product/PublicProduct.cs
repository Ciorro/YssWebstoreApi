using YssWebstoreApi.Models.DTOs.Accounts;

namespace YssWebstoreApi.Models.DTOs.Product
{
    public class PublicProduct
    {
        public required ulong Id { get; set; }
        public required DateTimeOffset CreatedAt { get; set; }
        public required DateTimeOffset UpdatedAt { get; set; }
        public required PublicAccount Account { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string SourceUrl { get; set; }
        public required float Rating { get; set; }
        public IList<string> Images { get; set; } = [];
        public IList<Tag> Tags { get; set; } = [];
        public OS? SupportedOS { get; set; }
    }
}
