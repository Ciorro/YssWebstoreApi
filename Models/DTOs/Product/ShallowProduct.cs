namespace YssWebstoreApi.Models.DTOs.Product
{
    public class ShallowProduct
    {
        public required ulong Id { get; set; }
        public required DateTimeOffset CreatedAt { get; set; }
        public required DateTimeOffset UpdatedAt { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required float Rating { get; set; }
        public HashSet<Tag> Tags { get; set; } = [];
    }
}
