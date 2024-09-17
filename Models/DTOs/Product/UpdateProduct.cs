namespace YssWebstoreApi.Models.DTOs.Product
{
    public class UpdateProduct
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public string? SourceUrl { get; set; }
    }
}
