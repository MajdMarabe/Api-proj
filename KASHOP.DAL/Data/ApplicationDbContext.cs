using KASHOP.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.Data
{
    public  class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Category> categories {  get; set; }
        public DbSet<CategoryTranslation> CategoryTrnaslations {  get; set; }


        public DbSet<Product> products { get; set; }
        public DbSet<ProductTranslation> ProductTranslations { get; set; }

        public DbSet<Cart> Carts { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }



        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor)
            :base(options) 
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>().ToTable("Users");
            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");

            /////
                 builder.Entity<Category>()
                .HasOne(c => c.CreatedBy)
                .WithMany()
                .HasForeignKey(c => c.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Category>()
                .HasOne(c => c.UpdatedBy)
                .WithMany()
                .HasForeignKey(c => c.UpdatedById)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Product>()
                .HasOne(p => p.CreatedBy)
                .WithMany()
                .HasForeignKey(p => p.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Product>()
                .HasOne(p => p.UpdatedBy)
                .WithMany()
                .HasForeignKey(p => p.UpdatedById)
                .OnDelete(DeleteBehavior.Restrict);

        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            if(_httpContextAccessor.HttpContext != null)
    {

        var entries = ChangeTracker.Entries<AuditableEntity>();
        var currentUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        foreach (var entry in entries)
        {
            if (entry.State == Microsoft.EntityFrameworkCore.EntityState.Added)
            {

                entry.Property(e => e.CreatedById).CurrentValue = currentUserId;
                entry.Property(e => e.CreatedOn).CurrentValue = DateTime.UtcNow;



            }
            else if (entry.State == Microsoft.EntityFrameworkCore.EntityState.Modified)
            {
                entry.Property(e => e.UpdatedById).CurrentValue = currentUserId;
                entry.Property(e => e.UpdatedOn).CurrentValue = DateTime.UtcNow;

            }
        }
    }
    return base.SaveChangesAsync(cancellationToken);
}
    }
}
