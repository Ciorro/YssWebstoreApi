namespace YssWebstoreApi.Api.DTO.Search
{
    public class SortOptions
    {
        public string? OrderBy { get; set; }
        public SortingOrder Order { get; set; }

        public enum SortingOrder
        {
            DESC, ASC
        }
    }
}
