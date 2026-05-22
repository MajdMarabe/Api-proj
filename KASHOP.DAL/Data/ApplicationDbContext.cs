using KASHOP.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.Data
{
    public  class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Category> categories {  get; set; }
        public DbSet<CategoryTranslation> CategoryTrnaslations {  get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>().ToTable("Users");
            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");

        }

    }
}
