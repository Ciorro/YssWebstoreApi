using System.ComponentModel.DataAnnotations;
using YssWebstoreApi.Models.Query;

namespace YssWebstoreApi.Models.DTOs.Post
{
    public class SearchPost : SearchOptions
    {
        public ulong? ProductId { get; set; }
        public string? AccountName { get; set; }
        public string? SearchQuery { get; set; }

        [Range(0, int.MaxValue)]
        public int? ContentLength { get; set; }
    }
}
