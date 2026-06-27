using KASHOP.BLL.Service;
using KASHOP.DAL.Repository;
using KASHOP.DAL.utils;
using Stripe;
using FileService = KASHOP.BLL.Service.FileService;

namespace KASHOP.PL.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection Services, IConfiguration Configuration)
        {

            /////// CategoryRepositry : to tell the program to consider ICategoryRepositry as CategoryRepositry
            Services.AddScoped<ICategoryRepository, CategoryRepository>();
            Services.AddScoped<ICategoryService, CategoryService>();

            Services.AddScoped<ISeedData, RoleSeedData>();
            //// product
            Services.AddScoped<IProductRepository, ProductRepository>();
            Services.AddScoped<IProductService, BLL.Service.ProductService>();

            Services.AddScoped<IFileService, FileService>();
            Services.AddTransient<IEmailSender, EmailSender>();
            Services.AddScoped<IOrderRepository,OrderRepository>();


            Services.AddScoped<IAuthenticationService, AuthenticationService>();
           Services.AddScoped<ICartRepository, CartRepository>();
           Services.AddScoped<ICartService, CartService>();
            //// stripe
            Services.Configure<StripeSettings>(Configuration.GetSection("Stripe"));
            StripeConfiguration.ApiKey = Configuration["Stripe:SecretKey"];

            Services.AddScoped<ICheckoutService,BLL.Service.CheckoutService>();

            return Services;
        }
    }
}
