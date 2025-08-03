using YssWebstoreApi.Entities;

namespace YssWebstoreApi.Persistance.Storage.Interfaces
{
    public interface IAvatarStorage
    {
        Task<Resource> UploadAvatar(Guid accountId, IFormFile file);
    }
}
