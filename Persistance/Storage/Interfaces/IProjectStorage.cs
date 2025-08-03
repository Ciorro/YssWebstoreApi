using YssWebstoreApi.Entities;

namespace YssWebstoreApi.Persistance.Storage.Interfaces
{
    public interface IProjectStorage
    {
        Task<Resource> UploadIcon(Guid projectId, IFormFile file);
        Task<Resource> UploadImage(Guid projectId, IFormFile file);
    }
}
