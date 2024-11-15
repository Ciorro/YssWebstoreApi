using System.Diagnostics.CodeAnalysis;

namespace YssWebstoreApi.Models.Query
{
    public class Page<T>
    {
        public required int PageNumber { get; set; }
        public required int PageSize { get; set; }
        public required int ItemCount { get; set; }
        public ICollection<T> Items { get; set; } = [];

        public Page() { }

        [SetsRequiredMembers]
        public Page(int pageNumber, int pageSize, int itemCount)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            ItemCount = itemCount;
        }
    }
}
