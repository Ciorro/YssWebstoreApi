namespace YssWebstoreApi.Services.Images
{
    public interface IImageService
    {
        Task SaveImageAs(string path, byte[] data, ImageProperties properties);
        Task SaveImageAs(string path, Stream stream, ImageProperties properties);
    }
}
