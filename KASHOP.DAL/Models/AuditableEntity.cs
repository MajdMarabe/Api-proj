using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.Models
{
    public enum EntityState
    {
       Active=1,
       Inactive=2,
    }
    public class AuditableEntity
    {
        public string CreatedById { get; set; }
        public DateTime CreatedOn { get; set; }
        public ApplicationUser CreatedBy { get; set; } 

        public string ? UpdatedById { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public ApplicationUser ? UpdatedBy { get; set; } 

        public EntityState Status { get; set; } = EntityState.Active;

    }
}
