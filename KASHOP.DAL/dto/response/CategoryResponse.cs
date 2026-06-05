using KASHOP.DAL.dto.request;
using KASHOP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.dto.response
{
    public class CategoryResponse
    {
        public int Id { get; set; }
     //   public List<CategoryTranslationRequest> Translations { get; set; }

        public string Name { get; set; }
        public string CreatedBy { get; set; }
}
}
