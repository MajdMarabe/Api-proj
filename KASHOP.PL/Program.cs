
using KASHOP.BLL.Mapping;
using KASHOP.BLL.Service;
using KASHOP.DAL.Data;
using KASHOP.DAL.Models;
using KASHOP.DAL.Repository;
using KASHOP.DAL.utils;
using KASHOP.PL.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Writers;
using System.Globalization;
using System.Text;
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
            builder.Services.AddDatabaseServices(builder.Configuration);
            /// Cors policy
           builder.Services.AddCorsPolicy();


            //lang
            builder.Services.AddLocalizationServices();
            //services
            builder.Services.AddApplicationServices();

          



            // identity 
            builder.Services.AddIdentityServices();

            //Authentication
            builder.Services.AddAuthenticationServices(builder.Configuration);

           
            MapsterConfig.MapsterConfigRegister();

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

            app.UseStaticFiles();
            app.MapControllers();
            app.UseCors(CorsPolicyExtensions.MyAllowSpecificOrigins);


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
