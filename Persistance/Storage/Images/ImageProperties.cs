namespace YssWebstoreApi.Persistance.Storage.Images
{
    public class ImageProperties
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public string Format { get; set; }

        public ImageProperties(int width, int height)
            : this(width, height, "jpg")
        { }

        public ImageProperties(int width, int height, string format)
        {
            Width = width;
            Height = height;
            Format = format;
        }

        public static readonly ImageProperties PostImage
            = new ImageProperties(1000, 500, "jpg");

        public static readonly ImageProperties AvatarImage
            = new ImageProperties(256, 256, "jpg");

        public static readonly ImageProperties ProjectIcon
            = new ImageProperties(128, 128, "png");
    }
}