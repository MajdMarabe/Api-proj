using KASHOP.DAL.dto.request;
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
        public List<CategoryTranslationRequest> Translations { get; set; }

    }
}
