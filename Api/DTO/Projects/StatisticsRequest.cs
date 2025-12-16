using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YssWebstoreApi.Api.DTO.Projects
{
    public class StatisticsRequest
    {
        public DateOnly? RangeStart { get; set; }
        public DateOnly? RangeEnd { get; set; }
    }
}
