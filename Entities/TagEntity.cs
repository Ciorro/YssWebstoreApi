using YssWebstoreApi.Entities.Interfaces;
using YssWebstoreApi.Entities.Tags;

namespace YssWebstoreApi.Entities
{
    public class TagEntity : IEntity
    {
        public Guid Id { get; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public required Tag Tag { get; set; }
    }
}
