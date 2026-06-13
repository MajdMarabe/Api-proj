using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.dto.response
{
    public class ProductResponse
    {
        public int Id { get; set; }
        public string CreatedBy { get; set; }
        public string Name { get; set; }
        public string MainImage { get; set; }

        public decimal Price { get; set; }


    }
}
