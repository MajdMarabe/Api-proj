using KASHOP.DAL.dto.response;
using KASHOP.DAL.Models;
using Mapster;
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

            TypeAdapterConfig<Product, ProductResponse>.NewConfig()
                .Map(dest => dest.CreatedBy, source => source.CreatedBy.UserName)
                .Map(dest => dest.Name, source => source.Translations.Where(t => t.Language == CultureInfo.CurrentCulture.Name)
                .Select(t => t.Name).FirstOrDefault())
                .Map(dest => dest.MainImage, source => $"http://localhost:5203/images/{source.MainImage}");








        }
    }
}
