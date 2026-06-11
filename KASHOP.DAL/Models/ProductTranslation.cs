using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.Models
{
    public class ProductTranslation
    {
        public int Id { get; set; }
        public string Language { get; set; } = "en";
        public string Name { get; set; }
        public string Description { get; set; }


       //relation 
        public int ProductId { get; set; }
        //navigation proparity
        public Product Product { get; set; }
    }
}
