using KASHOP.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.Data
{
    public  class ApplicationDbContext : DbContext
    {
        public DbSet<Category> categories {  get; set; }
        public DbSet<CategoryTranslation> CategoryTrnaslations {  get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options) { }
        

        
    }
}
