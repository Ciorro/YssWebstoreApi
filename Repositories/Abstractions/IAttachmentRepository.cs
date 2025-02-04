using YssWebstoreApi.Models.Abstractions;

namespace YssWebstoreApi.Repositories.Abstractions
{
    public interface IAttachmentRepository<T> : IRepository<T>
        where T : IEntity, IAttachment
    {
        Task<ulong?> CreateAndAttachAsync(T entity, Stream stream);
        Task<bool> DeleteAndDetachAsync(ulong id);
    }
}
