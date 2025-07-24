using SkiaSharp;
using YssWebstoreApi.Extensions;
using YssWebstoreApi.Persistance.Storage.Images;
using YssWebstoreApi.Persistance.Storage.Interfaces;

namespace YssWebstoreApi.Persistance.Storage
{
    public class ImageStorage : IImageStorage
    {
        private readonly IStorage _storage;

        public ImageStorage(IStorage storage)
        {
            _storage = storage;
        }

        public async Task Upload(string path, IFormFile file, ImageProperties imageProperties)
        {
            using var sourceBitmap = SKBitmap.Decode(file.OpenReadStream());
            using var processedBitmap = ResizeImage(
                sourceBitmap,
                imageProperties.Width,
                imageProperties.Height);

            SKEncodedImageFormat format = GetFormatFromString(imageProperties.Format);
            SKData processedImageData = processedBitmap.Encode(format, 90);

            await _storage.Upload(path, processedImageData.ToArray());
        }

        private SKBitmap ResizeImage(SKBitmap image, int width, int height)
        {
            float scale = 1f;
            float srcAspectRatio = (float)image.Width / image.Height;
            float dstAspectRatio = (float)width / height;

            if (srcAspectRatio < dstAspectRatio)
            {
                scale = (float)width / image.Width;
            }
            else
            {
                scale = (float)height / image.Height;
            }

            float scaledWidth = image.Width * scale;
            float scaledHeight = image.Height * scale;

            float x = (width - scaledWidth) / 2;
            float y = (height - scaledHeight) / 2;

            var result = new SKBitmap(width, height);
            using (var canvas = new SKCanvas(result))
            {
                canvas.DrawBitmap(image, new SKRect(x, y, x + scaledWidth, y + scaledHeight));
            }
            return result;
        }

        private SKEncodedImageFormat GetFormatFromString(string format)
        {
            switch (format.ToLower().TrimStart('.'))
            {
                case "jpg":
                case "jpeg":
                    return SKEncodedImageFormat.Jpeg;
                case "png":
                    return SKEncodedImageFormat.Png;
                case "gif":
                    return SKEncodedImageFormat.Gif;
                case "webp":
                    return SKEncodedImageFormat.Webp;
                case "bmp":
                    return SKEncodedImageFormat.Bmp;
                default:
                    throw new ArgumentException(nameof(format), "Unsupported image format.");
            }
        }
    }
}
