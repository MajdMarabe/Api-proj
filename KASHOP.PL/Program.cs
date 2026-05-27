
using KASHOP.BLL.Service;
using KASHOP.DAL.Data;
using KASHOP.DAL.Models;
using KASHOP.DAL.Repository;
using KASHOP.DAL.utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Writers;
using System.Globalization;
using System.Threading.Tasks;

namespace KASHOP.PL
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();


            ////DB

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefultConnection"));
            }

                );

            //lang

            builder.Services.AddLocalization(options => options.ResourcesPath = "");
            const string defaultCulture = "en-GB";

            var supportedCultures = new[]
            {
              new CultureInfo(defaultCulture),
              new CultureInfo("ar"),
            };


            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture(defaultCulture);
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                options.RequestCultureProviders = new List<IRequestCultureProvider>
                {
                  new QueryStringRequestCultureProvider()
                };

                options.AddInitialRequestCultureProvider(new AcceptLanguageHeaderRequestCultureProvider());/// header Accept-Language
            });
            /////// CategoryRepositry : to tell the program to consider ICategoryRepositry as CategoryRepositry
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            //////
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<ISeedData, RoleSeedData>();



            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            ////
            ///
            builder.Services.AddTransient<IEmailSender, EmailSender>();


            //////
            var app = builder.Build();
            app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            using ( var scope = app.Services.CreateScope()) { 
                var services = scope.ServiceProvider;
                var seeders = services.GetServices<ISeedData>();
                foreach (var seeder in seeders)
                {

                    await seeder.SeedData();
                }
            
            }

            app.Run();
        }
    }
}
