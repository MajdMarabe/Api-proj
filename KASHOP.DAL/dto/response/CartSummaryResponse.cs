using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.dto.response
{
    public class CartSummaryResponse
    {
        public List<AddToCartResponse> Items { get; set; }
    }
}
