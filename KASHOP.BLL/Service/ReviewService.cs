using KASHOP.DAL.dto.request;
using KASHOP.DAL.Models;
using KASHOP.DAL.Repository;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.BLL.Service
{
    public class ReviewService : IReviewService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IReviewRepositry _reviewRepositry;

        public ReviewService(IOrderRepository orderRepository, IReviewRepositry  reviewRepositry)
        {
            _orderRepository = orderRepository;
            _reviewRepositry = reviewRepositry;
        }


        public async Task<bool> AddReview(string userId, AddReviewRequest request)
        {
            var purchasedOrder = await _orderRepository.GetOne(
                o => o.UserId == userId &&
                o.OrderStatus == DAL.Models.OrderStatusEnum.Delivered &&
                o.OrderItems.Any(oi => oi.ProductId == request.ProductId),
                new[]
                {
                    nameof(Order.OrderItems)
                });
            if (purchasedOrder == null) return false;

            var AlreadyReviews = await _reviewRepositry.GetOne(
                r=> r.UserId== userId && r.ProductId ==request.ProductId
                );
            if (AlreadyReviews != null) return false;
            var review =request.Adapt<Review>();
            review.UserId = userId;

            await _reviewRepositry.CreateAsync(review);
            return true;



        }
    }
}
