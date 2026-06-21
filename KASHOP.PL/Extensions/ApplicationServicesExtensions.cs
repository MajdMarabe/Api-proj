using KASHOP.BLL.Service;
using KASHOP.DAL.Repository;
using KASHOP.DAL.utils;

namespace KASHOP.PL.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection Services)
        {

            /////// CategoryRepositry : to tell the program to consider ICategoryRepositry as CategoryRepositry
            Services.AddScoped<ICategoryRepository, CategoryRepository>();
            Services.AddScoped<ICategoryService, CategoryService>();

            Services.AddScoped<ISeedData, RoleSeedData>();
            //// product
            Services.AddScoped<IProductRepository, ProductRepository>();
            Services.AddScoped<IProductService, ProductService>();

            Services.AddScoped<IFileService, FileService>();
            Services.AddTransient<IEmailSender, EmailSender>();


            Services.AddScoped<IAuthenticationService, AuthenticationService>();
           Services.AddScoped<ICartRepository, CartRepository>();
           Services.AddScoped<ICartService, CartService>();

            return Services;
        }
    }
}
