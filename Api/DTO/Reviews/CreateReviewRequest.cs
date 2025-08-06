using System.ComponentModel.DataAnnotations;

namespace YssWebstoreApi.Api.DTO.Reviews
{
    public class CreateReviewRequest
    {
        [Range(1, 5)]
        public required int Rate { get; set; }
        public string? Content { get; set; }
    }
}
