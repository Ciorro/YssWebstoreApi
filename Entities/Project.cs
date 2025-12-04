using YssWebstoreApi.Entities.Interfaces;
using YssWebstoreApi.Entities.Tags;

namespace YssWebstoreApi.Entities
{
    public class Project : IEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime ReleasedAt { get; set; }
        public required Guid AccountId { get; set; }
        public required string Name { get; set; }
        public required string Slug { get; set; }
        public required string Description { get; set; }
        public Resource? Icon { get; set; }
        public Resource? Banner { get; set; }
        public TagCollection Tags { get; set; } = [];
        public IList<Resource> Images { get; set; } = [];
        public IList<Package> Packages { get; set; } = [];
        public bool IsPinned { get; set; }

        public void RegenerateOSTags()
        {
            Tags.RemoveGroup("os");

            var supportedOs = Packages
                .Select(x => x.TargetOS)
                .Distinct()
                .Select(os => os.ToString().ToLower());

            Tags.AddValuesToGroup("os", supportedOs);
        }
    }
}
