using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace YssWebstoreApi.Models.DTOs.Search
{
    public class PageOptions
    {
        [FromQuery(Name = "Page"), Range(1, int.MaxValue)]
        public int PageNumber { get; set; } = 1;

        [Range(1, int.MaxValue)]
        public int PageSize { get; set; } = 10;
    }
}
