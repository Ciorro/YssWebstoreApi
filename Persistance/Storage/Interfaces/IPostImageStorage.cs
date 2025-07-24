namespace YssWebstoreApi.Persistance.Storage.Interfaces
{
    public interface IPostImageStorage
    {
        Task<string> UploadCoverImage(Guid postId, IFormFile file);
    }
}
