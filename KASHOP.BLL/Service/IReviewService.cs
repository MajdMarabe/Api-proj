using KASHOP.DAL.dto.request;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.BLL.Service
{
    public interface IReviewService
    {
        Task<bool> AddReview(string userId, AddReviewRequest request);
    }
}
