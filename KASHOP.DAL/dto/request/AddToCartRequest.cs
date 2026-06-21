using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.dto.request
{
    public class AddToCartRequest
    {
        public int ProductId { get; set; }
        public int count { get; set; } = 1;
    }
}
