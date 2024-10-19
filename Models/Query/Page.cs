using System.Diagnostics.CodeAnalysis;

namespace YssWebstoreApi.Models.Query
{
    public class Page<T>
    {
        public required int PageNumber { get; set; }
        public required int PageSize { get; set; }
        public required int TotalCount { get; set; }
        public ICollection<T> Items { get; set; } = [];

        public Page() { }

        [SetsRequiredMembers]
        public Page(int pageNumber, int pageSize, int totalCount)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalCount = totalCount;
        }
    }
}
