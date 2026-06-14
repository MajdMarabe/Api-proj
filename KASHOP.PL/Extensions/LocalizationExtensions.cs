using Microsoft.AspNetCore.Localization;
using System.Globalization;

namespace KASHOP.PL.Extensions
{
    public static class LocalizationExtensions
    {
        public static IServiceCollection AddLocalizationServices(this IServiceCollection Services)
        {

            Services.AddLocalization(options => options.ResourcesPath = "");
            const string defaultCulture = "en";

            var supportedCultures = new[]
            {
              new CultureInfo(defaultCulture),
              new CultureInfo("ar"),
            };


            Services.Configure<RequestLocalizationOptions>(options =>
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
            return Services;
        }
    }
}
