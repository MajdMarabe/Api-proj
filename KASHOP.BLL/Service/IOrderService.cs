using KASHOP.DAL.dto.request;
using KASHOP.DAL.dto.response;
using KASHOP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.BLL.Service
{
    public interface IOrderService
    {
        // user
        Task<List<OrderResponse>> GetUserOrders(string userId);
        Task<OrderDetailsResponse?> GetUserOrder(string userId, int orderId);
        Task<bool> CancelOrder(string userId, int orderId);

        // Admin
        Task<List<OrderDetailsResponse>> GetAllOrders(OrderStatusEnum status);
        Task<bool> ChangeOrderStatus(int orderId, ChangeOrderStatusRequest request);

    }
}
