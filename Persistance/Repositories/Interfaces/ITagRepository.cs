using YssWebstoreApi.Entities;
using YssWebstoreApi.Entities.Tags;

namespace YssWebstoreApi.Persistance.Repositories.Interfaces
{
    public interface ITagRepository : IRepository<TagEntity>
    {
        Task<IList<TagEntity>> GetAndInsert(IEnumerable<Tag> tags);
    }
}
