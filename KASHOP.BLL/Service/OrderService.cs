using KASHOP.DAL.dto.request;
using KASHOP.DAL.dto.response;
using KASHOP.DAL.Models;
using KASHOP.DAL.Repository;
using Mapster;
using Stripe.Climate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Order = KASHOP.DAL.Models.Order;
using Product = KASHOP.DAL.Models.Product;


namespace KASHOP.BLL.Service
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepositry;

        public OrderService(IOrderRepository orderRepositry) {
            _orderRepositry= orderRepositry;
        }
        public async Task<List<OrderResponse>> GetUserOrders(string userId)
        {
         var orders = await _orderRepositry.GetAllAsync(o => o.UserId == userId, new[]
         {
             nameof(Order.OrderItems),
             $"{nameof(Order.OrderItems)}.{nameof(OrderItem.Product)}",
             $"{nameof(Order.OrderItems)}.{nameof(OrderItem.Product)}.{nameof(Product.Translations)}"
         });

            return orders.Adapt<List<OrderResponse>>();
        }


        public async Task<OrderDetailsResponse?> GetUserOrder(string userId, int orderId)
        {

            var order = await _orderRepositry.GetOne(
                o=>o.UserId == userId && o.Id == orderId,
         new[]
         {
             nameof(Order.OrderItems),
             $"{nameof(Order.OrderItems)}.{nameof(OrderItem.Product)}",
             $"{nameof(Order.OrderItems)}.{nameof(OrderItem.Product)}.{nameof(Product.Translations)}"
         }
                );

            return order.Adapt<OrderDetailsResponse>();
        }

        public async  Task<bool> CancelOrder(string userId, int orderId)
        {
            var order = await _orderRepositry.GetOne(
                           o => o.UserId == userId && o.Id == orderId);
            if (order == null) return false;
            if(order.OrderStatus != OrderStatusEnum.pending) return false;

            order.OrderStatus = OrderStatusEnum.Cannceled;
            return await _orderRepositry.UpdateAsync(order);

         
          
        }

        public async Task<List<OrderDetailsResponse>> GetAllOrders(OrderStatusEnum status)
        {
            var orders = await _orderRepositry.GetAllAsync(
                           o => o.OrderStatus == status );

            return orders.Adapt<List<OrderDetailsResponse>>();
        }

        public async Task<bool> ChangeOrderStatus(int orderId, ChangeOrderStatusRequest request)
        {
            var order = await _orderRepositry.GetOne(o=>o.Id == orderId);
            if (order == null) return false;
            if(order.OrderStatus == OrderStatusEnum.Cannceled|| order.OrderStatus == OrderStatusEnum.Delivered) return false;

            if((int)request.Status != (int)order.OrderStatus+1) return false;
            order.OrderStatus = request.Status;
            return await  _orderRepositry.UpdateAsync(order);
        }
    }
}
