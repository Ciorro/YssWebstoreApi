using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace YssWebstoreApi.Api.DTO.Search
{
    public class PageOptions
    {
        [FromQuery(Name = "Page"), Range(1, int.MaxValue)]
        public int PageNumber { get; set; } = 1;

        [FromQuery, Range(1, int.MaxValue)]
        public int PageSize { get; set; } = 10;

        public int GetOffset()
        {
            return (PageNumber - 1) * PageSize;
        }
    }
}
