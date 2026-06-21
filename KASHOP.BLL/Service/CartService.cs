using KASHOP.DAL.dto.request;
using KASHOP.DAL.dto.response;
using KASHOP.DAL.Models;
using KASHOP.DAL.Repository;
using Mapster;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.BLL.Service
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepositry;

        public CartService(ICartRepository cartRepository, IProductRepository productRepositry)
        {
            _cartRepository = cartRepository;
            _productRepositry = productRepositry;
        }
        public async Task<bool>AddToCart(AddToCartRequest request, string userId)
        {
            var ExistingItem = await _cartRepository.GetOne(p => p.ProductId == request.ProductId && p.UserId == userId);
            var product = await _productRepositry.GetOne(p => p.Id == request.ProductId);

            //
            var currntCount = ExistingItem?.count ?? 0;
            if (currntCount + request.count > product.Quantity) return false ;


                if (ExistingItem != null)

                {
                
                    ExistingItem.count += request.count;

                    await _cartRepository.UpdateAsync(ExistingItem);
                

                }
            else
            {

                    var cartItem = request.Adapt<Cart>();
                cartItem.UserId = userId;

                await _cartRepository.CreateAsync(cartItem);
            

            }
            return true;
        }

        public async Task<bool> ClearCart(string userId)
        {
var items = await _cartRepository.GetAllAsync(p => p.UserId == userId);
            if (items == null) return false;
          return await _cartRepository.DeleteRangeAsync(items);
        }



        public async Task<List<AddToCartResponse>> GetCartItems(string userId)
        {
            var items = await  _cartRepository.GetAllAsync(p => p.UserId == userId, new string[] {nameof(Cart.Product),
                
                $"{nameof(Cart.Product)}.{nameof(Product.Translations)}"});
           return items.Adapt<List<AddToCartResponse>>();

        }

        public async Task<bool> RemoveItem(int productId, string userId)
        {
            var product = await _cartRepository.GetOne(p => p.ProductId == productId && p.UserId == userId);
            if (product == null) return false;
            var result = await _cartRepository.DeleteAsync(product);
            return result;
        }

        public async Task<bool> UpdateQantity(int productId, int count, string userId)
        {
            var ExistingItem = await  _cartRepository.GetOne(p => p.ProductId == productId && p.UserId == userId);
            if (ExistingItem == null) return false;
            var product = await _productRepositry.GetOne(p => p.Id == productId);
            if (count > product.Quantity) return false; 

            ExistingItem.count = count;

            return await _cartRepository.UpdateAsync(ExistingItem);
        }
    }
}
