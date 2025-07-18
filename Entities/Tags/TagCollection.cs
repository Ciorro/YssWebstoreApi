using System.Collections;

namespace YssWebstoreApi.Entities.Tags
{
    public class TagCollection : ICollection<Tag>
    {
        private readonly HashSet<Tag> _tags = new HashSet<Tag>();

        public TagCollection() 
        { }

        public TagCollection(IEnumerable<Tag> tags)
        {
            _tags = new HashSet<Tag>(tags);
        }

        public TagCollection(IEnumerable<string> tags)
        {
            _tags = new HashSet<Tag>(tags.Select(Tag.Parse));
        }
        public int Count
        {
            get => _tags.Count;
        }

        bool ICollection<Tag>.IsReadOnly
        {
            get => false;
        }

        public void Add(Tag item)
            => _tags.Add(item);

        public bool Contains(Tag item)
            => _tags.Contains(item);

        public bool Remove(Tag item)
            => _tags.Remove(item);

        public void Clear()
            => _tags.Clear();

        public void CopyTo(Tag[] array, int arrayIndex)
            => _tags.CopyTo(array, arrayIndex);

        public IEnumerator<Tag> GetEnumerator()
            => _tags.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => _tags.GetEnumerator();

        public IEnumerable<string> GetValuesFromGroup(string tagGroup)
            => _tags.Where(x => x.Group == tagGroup).Select(x => x.Value);
    }
}
