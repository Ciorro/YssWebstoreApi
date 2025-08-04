namespace YssWebstoreApi.Api.DTO.Reviews
{
    public class UpdateReviewRequest
    {
        public required int Rate { get; set; }
        public string? Content { get; set; }
    }
}
