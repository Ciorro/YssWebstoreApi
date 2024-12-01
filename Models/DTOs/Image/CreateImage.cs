using Microsoft.AspNetCore.Mvc;
using YssWebstoreApi.Models.Validation;

namespace YssWebstoreApi.Models.DTOs.Image
{
    public class CreateImage
    {
        public string? Title { get; set; }

        [MaxContentLength(3_000_000)]
        public required IFormFile File { get; set; }
    }
}
