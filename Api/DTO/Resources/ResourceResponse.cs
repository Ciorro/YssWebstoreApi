namespace YssWebstoreApi.Api.DTO.Resources
{
    public class ResourceResponse
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public required string Url { get; set; }
    }
}
