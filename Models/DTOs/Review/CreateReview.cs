using System.ComponentModel.DataAnnotations;

namespace YssWebstoreApi.Models.DTOs.Review
{
    public class CreateReview
    {
        [Range(1, 5)]
        public required int Rate { get; set; }
        public string? Content { get; set; }
    }
}
