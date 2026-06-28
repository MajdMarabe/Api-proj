using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.dto.request
{
    public class ProductUpdateRequest
    {
        public decimal ? Price { get; set; }
        public decimal ? Discount { get; set; }
        public int ? Quantity { get; set; }
        public IFormFile ? MainImage { get; set; }

        public int ? CategoryId { get; set; }

        public List<IFormFile> ? SubImages { get; set; }
        public List<IFormFile>? NewImges { get; set; }

        public List<ProductTranslationsRequest> ? Translations { get; set; }
    }
}
