using YssWebstoreApi.Models.Validation;

namespace YssWebstoreApi.Models.DTOs.Resources
{
    public class UploadImage
    {
        [MaxContentLength(4 *  1024 * 1024)]
        public required IFormFile ImageFile { get; set; }
    }
}
