using KASHOP.DAL.dto.request;
using KASHOP.DAL.dto.response;
using KASHOP.DAL.Models;
using KASHOP.DAL.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Stripe.Checkout;
using Stripe;

namespace KASHOP.BLL.Service
{
    public class CheckoutService : ICheckoutService
    {
        private readonly ICartRepository _cartRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOrderRepository _orderRepositry;
        private readonly ICartService _cartService;
        private readonly IProductRepository _productRepository;
        private readonly IEmailSender _emailSender;

        



        public CheckoutService(IEmailSender emailSender,ICartRepository cartRepository, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, IOrderRepository orderRepositry,ICartService cartService,IProductRepository productRepository)
        {
            _cartRepository = cartRepository;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _orderRepositry = orderRepositry;
            _cartService = cartService;
            _productRepository = productRepository;
            _emailSender = emailSender;
        }


        public async Task<CheckoutResponse> ProcessCheckout(string userId, CheckoutRequest request)
        {
            var cartItems = await _cartRepository.GetAllAsync(c => c.UserId == userId,new[] { nameof(Cart.Product), "Product.Translations" });
            if (!cartItems.Any())
            {
                return new CheckoutResponse
                {
                    Success = false,
                    Error = "Cart is empty."
                };
            }
            // street, city, phone number 
            var user = await _userManager.FindByIdAsync(userId);
            var city = request.City ?? user.City;
            if (city == null) {
                return new CheckoutResponse
                {
                    Success = false,
                    Error = "city is required"
                };


            }
            var street = request.Street ?? user.Street;
            if (street == null)
            {
                return new CheckoutResponse
                {
                    Success = false,
                    Error = "street is required"
                };


            }
            var phoneNumber = request.PhoneNumber ?? user.PhoneNumber;
            if (phoneNumber == null)
            {
                return new CheckoutResponse
                {
                    Success = false,
                    Error = "phoneNumber is required"
                };

            }
            /////
            ////check Quantity
            foreach (var item in cartItems) {

                if (item.count > item.Product.Quantity)
                {
                    return new CheckoutResponse
                    {
                        Success = false,
                        Error = "dosn't have enough stock ",
                    };
                }

            }
            ////// order
            ///
            var order = new Order()
            {

                UserId = userId,
                City = city,
                Street = street,
                PhoneNumber = phoneNumber,
                PaymentMethod = request.PaymentMethod,
                AmountPaid = cartItems.Sum(c => c.Product.Price * c.count),
                OrderItems = cartItems.Select(c =>
                 new OrderItem
                 {
                     ProductId = c.ProductId,
                     Quantity = c.count,
                     UnitPrice =c.Product.Price,
                     TotalPrice = c.Product.Price*c.count,

                 }).ToList()
            };
            await _orderRepositry.CreateAsync(order);
            
            ////// PaymentMethod
            if (request.PaymentMethod == PaymentMethodEnum.Cash)
            {
                return new CheckoutResponse
                {
                    Success = false,
                    Error = "Cash",
                };
            }


            if (request.PaymentMethod == PaymentMethodEnum.Visa)
            {
                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string> { "card" },
                    Mode = "payment",
                    SuccessUrl =$"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/api/checkouts/success?sessionId={{CHECKOUT_SESSION_ID}}",
                    CancelUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/api/checkout/cancel",

                    LineItems = new List<SessionLineItemOptions>()
                };

                foreach (var item in cartItems) {
                    options.LineItems.Add(



                         new SessionLineItemOptions
                         {
                             PriceData = new SessionLineItemPriceDataOptions
                             {
                                 Currency = "USD",
                                 ProductData = new SessionLineItemPriceDataProductDataOptions
                                 {
                                     Name = item.Product.Translations.FirstOrDefault(t => t.Language == "en")?.Name,
                                 },
                                 UnitAmount =(long)( item.Product.Price*100),
                             },
                             Quantity =item.count,
                         }

                    );
                
                }

                var service = new SessionService();
                var session = service.Create(options);
                order.StripeSessionId = session.Id;
                await _orderRepositry.UpdateAsync(order);

                return new CheckoutResponse
                {
                    Success = true,
                    StripeUrl = session.Url, 
                };
            }
            return new CheckoutResponse
            {
                Success = false,
                Error = "Invalid payment method"
            };
        }

        public async Task<CheckoutResponse> HandelSuccess(string sessionId)
        {
            var order = _orderRepositry.GetOne(o => o.StripeSessionId == sessionId, new[] {
         "OrderItems",
         "OrderItems.Product",
         "OrderItems.Product.Translations"
        }).Result;

            order.OrderStatus = OrderStatusEnum.Paid;
            await _orderRepositry.UpdateAsync(order);
            await _cartService.ClearCart(order.UserId);
            /// update the Quantity
            /// 
            var LowStockProduct = await _productRepository.DecreaseQuantityAsync(order.OrderItems);
            foreach (var product in LowStockProduct)
            {
                await _emailSender.SendEmailAsync("s12027646@stu.najah.edu",
                    "low stock alert",
                    $"<h2>product{product.Translations.FirstOrDefault(t => t.Language == "en").Name} current quantity:{product.Quantity}</h2>");

            }
             ////
            ///send email to the user 
            var user = _userManager.FindByIdAsync(order.UserId).Result;

            await _emailSender.SendEmailAsync(user.Email,"order confirmed","<h2> your order has been placed successfully</h2>");
            return new CheckoutResponse
            {
                Success = true
            };
        }
    }
} 