using KASHOP.DAL.dto.request;
using KASHOP.DAL.dto.response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.BLL.Service
{
    public interface ICartService
    {
        public Task<bool> AddToCart(AddToCartRequest request, string userId);
        public Task<List<AddToCartResponse>> GetCartItems(string userId);
        public Task<bool> UpdateQantity(int productId, int count, string userId);
        public Task<bool> RemoveItem(int productId, string userId);
        public Task<bool> ClearCart(string userId);


    }
}
 