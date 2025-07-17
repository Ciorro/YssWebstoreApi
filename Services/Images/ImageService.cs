using SkiaSharp;
using YssWebstoreApi.Extensions;
using YssWebstoreApi.Persistance.Storage;

namespace YssWebstoreApi.Services.Images
{
    public class ImageService : IImageService
    {
        private readonly IFileStorage _fileStorage;

        public ImageService(IFileStorage fileStorage)
        {
            _fileStorage = fileStorage;
        }

        public async Task SaveImageAs(string path, byte[] data, ImageProperties properties)
        {
            using (var ms = new MemoryStream(data))
            {
                await SaveImageAs(path, ms, properties);
            }
        }

        public async Task SaveImageAs(string path, Stream stream, ImageProperties properties)
        {
            using (var bitmap = SKBitmap.Decode(stream))
            {
                SKBitmap scaledImage = ResizeImage(bitmap, properties.Width, properties.Height);

                using (var ms = new MemoryStream())
                {
                    if (scaledImage.Encode(ms, properties.Format.ToSkiaFormat(), 80))
                    {
                        await _fileStorage.Create(path, ms);
                    }
                }
            }
        }

        private SKBitmap ResizeImage(SKBitmap image, int width, int height)
        {
            float scale = 1f;
            float srcAspectRatio = (float)image.Width / image.Height;
            float dstAspectRatio = (float)width / height;

            if (srcAspectRatio > dstAspectRatio)
            {
                scale = (float)image.Width / width;
            }
            else
            {
                scale = (float)image.Height / height;
            }

            float x = width / 2 - image.Width * scale / 2;
            float y = height / 2 - image.Height * scale / 2;

            using (var result = new SKBitmap(width, height))
            using (var canvas = new SKCanvas(result))
            {
                canvas.DrawBitmap(image, new SKRect(x, y, width, height));
                return result;
            }
        }
    }
}
