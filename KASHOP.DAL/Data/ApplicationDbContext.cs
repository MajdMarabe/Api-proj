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

        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            if(_httpContextAccessor.HttpContext != null)
            {

                var entries = ChangeTracker.Entries<AuditableEntity>();
                var currentUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                foreach (var entry in entries)
                {
                    if (entry.State == EntityState.Added)
                    {

                        entry.Property(e => e.CreatedById).CurrentValue = currentUserId;
                        entry.Property(e => e.CreatedOn).CurrentValue = DateTime.UtcNow;
                       // entry.Property(e => e.CreatedBy).CurrentValue = currentUser;



                    }
                    else if (entry.State == EntityState.Modified)
                    {
                        entry.Property(e => e.UpdatedById).CurrentValue = currentUserId;
                        entry.Property(e => e.UpdatedOn).CurrentValue = DateTime.UtcNow;
                     //  entry.Property(e => e.UpdatedBy).CurrentValue = currentUser;

                    }
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }

    }
}
