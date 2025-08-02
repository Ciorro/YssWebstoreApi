namespace YssWebstoreApi.Persistance.Storage.Interfaces
{
    public interface IProjectStorage
    {
        Task<string> UploadIcon(Guid projectId, IFormFile file);
        Task<string> UploadImage(Guid projectId, IFormFile file);
    }
}
