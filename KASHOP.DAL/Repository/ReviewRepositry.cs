using KASHOP.DAL.Data;
using KASHOP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.Repository
{
    public class ReviewRepositry : GenericRepository<Review>, IReviewRepositry
    {
        public ReviewRepositry(ApplicationDbContext context) : base(context) { }// sent to GenericRepository constructor

    }
}
