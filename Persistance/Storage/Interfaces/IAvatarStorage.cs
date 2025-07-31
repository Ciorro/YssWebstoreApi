namespace YssWebstoreApi.Persistance.Storage.Interfaces
{
    public interface IAvatarStorage
    {
        Task<string> UploadAvatar(Guid accountId, IFormFile file);
    }
}
