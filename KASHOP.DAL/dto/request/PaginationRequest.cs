using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.dto.request
{
    public class PaginationRequest
    {
        public int page { get; set; } = 1;
        public int Limit { get; set; } = 10;
        public string ? Search {  get; set; }

    }
}
