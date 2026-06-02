
using KASHOP.BLL.Service;
using KASHOP.DAL.Data;
using KASHOP.DAL.Models;
using KASHOP.DAL.Repository;
using KASHOP.DAL.utils;
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
            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

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
            /// Cors policy
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  policy =>
                                  {
                                      policy.AllowAnyOrigin()
                                            .AllowAnyMethod()
                                            .AllowAnyHeader();  
                                  });
            });

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
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(
                Options =>
                {
                    Options.User.RequireUniqueEmail = true;
                    ///password policy
                    Options.Password.RequireDigit = true;  
                    Options.Password.RequireLowercase = true;
                    Options.Password.RequireUppercase = true;
                    Options.Password.RequireNonAlphanumeric = true;
                  //  Options.Password.RequiredLength = 10;
                    //
                    Options.Lockout.MaxFailedAccessAttempts = 5;
                    Options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10); 
                }

                )
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();



            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })

                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = builder.Configuration["Jwt:Issuer"],
                            ValidAudience = builder.Configuration["Jwt:Audience"],
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
                        };
                    });

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
            app.UseCors(MyAllowSpecificOrigins);


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
