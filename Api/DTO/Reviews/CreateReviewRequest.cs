using Microsoft.AspNetCore.Mvc;

namespace YssWebstoreApi.Api.DTO.Reviews
{
    public class CreateReviewRequest
    {
        public required int Rate { get; set; }
        public string? Content { get; set; }
    }
}
