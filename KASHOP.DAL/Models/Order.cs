using KASHOP.DAL.dto.request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.Models
{
    public enum OrderStatusEnum {
        pending =1,
        Approved =2,
        Shipped =3,
        Delivered =4,
        Cannceled =5, 
        Paid=6,
        }
    public class Order
    {
        public int Id { get; set; }

        public PaymentMethodEnum PaymentMethod { get; set; }

        public DateTime OrderDate { get; set; }= DateTime.Now;
        public DateTime ? ShippedDate {  get; set; }

        public OrderStatusEnum OrderStatus { get; set; }
        public string ? StripeSessionId { get; set; }/// the session we created for this order in checkout
        public decimal? AmountPaid { get; set; }

        public string City { get; set; }
        public string Street { get; set; }
        public string PhoneNumber { get; set; }
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>(); 
    }
}
