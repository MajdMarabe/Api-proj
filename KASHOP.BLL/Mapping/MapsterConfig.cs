using KASHOP.DAL.dto.request;
using KASHOP.DAL.dto.response;
using KASHOP.DAL.Models;
using Mapster;
using Stripe;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.BLL.Mapping
{
    public static class MapsterConfig
    {
        public static void MapsterConfigRegister()
        {
            TypeAdapterConfig<Category, CategoryResponse>.NewConfig()
                .Map(dest => dest.Id, source => source.Id)
                .Map(dest => dest.CreatedBy, source => source.CreatedBy.UserName)

                .Map(dest => dest.Name, source => source.Translations.Where(t => t.Language == CultureInfo.CurrentCulture.Name)
               .Select(t => t.Name).FirstOrDefault());

            TypeAdapterConfig<DAL.Models.Product, ProductResponse>.NewConfig()
                .Map(dest => dest.CreatedBy, source => source.CreatedBy.UserName)
                .Map(dest => dest.Name, source => source.Translations.Where(t => t.Language == CultureInfo.CurrentCulture.Name)
                .Select(t => t.Name).FirstOrDefault())
                .Map(dest => dest.MainImage, source => $"http://localhost:5203/images/{source.MainImage}")
                .Map(dest => dest.SubImages,
                 src => src.SubImages.Select(i => $"http://localhost:5203/images/{i.ImagePath}")
                 );




            TypeAdapterConfig<ProductUpdateRequest, DAL.Models.Product>
              .NewConfig()
              .Ignore(x => x.MainImage)
              .Ignore(x => x.SubImages)
              .Ignore(x => x.Translations)
             .IgnoreNullValues(true);

    
            TypeAdapterConfig<Cart, AddToCartResponse>.NewConfig()
                .Map(dest => dest.ProductName, source => source.Product.Translations.Where(t => t.Language == CultureInfo.CurrentCulture.Name)
               .Select(t => t.Name).FirstOrDefault())
                .Map(dest => dest.ProductImage, source => $"http://localhost:5203/images/{source.Product.MainImage}")
                .Map(dest => dest.Price, source => source.Product.Price)
                .Map(dest => dest.Count, source => source.count);









        }
    }
}
