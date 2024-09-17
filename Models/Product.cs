namespace YssWebstoreApi.Models
{
    public class Product 
    {
        public uint Id { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public uint? AccountId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? SourceUrl { get; set; }
    }
}
