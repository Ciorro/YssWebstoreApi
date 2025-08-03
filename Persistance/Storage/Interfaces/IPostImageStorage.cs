using YssWebstoreApi.Entities;

namespace YssWebstoreApi.Persistance.Storage.Interfaces
{
    public interface IPostImageStorage
    {
        Task<Resource> UploadCoverImage(Guid postId, IFormFile file);
    }
}
