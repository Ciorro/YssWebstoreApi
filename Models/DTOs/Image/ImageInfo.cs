using YssWebstoreApi.Models.DTOs.Accounts;

namespace YssWebstoreApi.Models.DTOs.Image
{
    public class ImageInfo
    {
        public string? Title { get; set; }
        public string? Url { get; set; }
        public PublicAccount? Account { get; set; }
    }
}
