using YssWebstoreApi.Api.Validation;

namespace YssWebstoreApi.Api.DTO.Resources
{
    public class UploadImage
    {
        [MaxContentLength(4 * 1024 * 1024)]
        public required IFormFile ImageFile { get; set; }
    }
}
