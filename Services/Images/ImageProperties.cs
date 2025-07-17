namespace YssWebstoreApi.Services.Images
{
    public class ImageProperties
    {
        public ImageFormat Format { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public ImageProperties(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }
}