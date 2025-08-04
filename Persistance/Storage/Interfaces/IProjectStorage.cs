using YssWebstoreApi.Entities;
using YssWebstoreApi.Persistance.Storage.Packages;

namespace YssWebstoreApi.Persistance.Storage.Interfaces
{
    public interface IProjectStorage
    {
        Task<Resource> UploadIcon(Guid projectId, IFormFile file);
        Task<Resource> UploadImage(Guid projectId, IFormFile file);
        Task<Resource> UploadPackage(Guid projectId, PackageInfo packageInfo, IFormFile file);
        Task DeletePackage(string path);
    }
}
