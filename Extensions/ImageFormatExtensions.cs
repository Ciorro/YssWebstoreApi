using SkiaSharp;
using YssWebstoreApi.Services.Images;

namespace YssWebstoreApi.Extensions
{
    public static class ImageFormatExtensions
    {
        public static SKEncodedImageFormat ToSkiaFormat(this ImageFormat imageFormat)
        {
            return imageFormat switch
            {
                ImageFormat.Png => SKEncodedImageFormat.Png,
                ImageFormat.Jpeg => SKEncodedImageFormat.Jpeg,
                _ => throw new NotImplementedException()
            };
        }

        public static ImageFormat FromSkiaFormat(this SKEncodedImageFormat imageFormat)
        {
            return imageFormat switch
            {
                SKEncodedImageFormat.Png => ImageFormat.Png,
                SKEncodedImageFormat.Jpeg => ImageFormat.Jpeg,
                _ => throw new NotImplementedException()
            };
        }
    }
}
