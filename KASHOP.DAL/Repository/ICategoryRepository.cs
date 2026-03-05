using KASHOP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.Repository
{
    public interface ICategoryRepository
    {
         public Task <List<Category>> GetAllAsync();
        public Task< Category> CreateAsync(Category category);
    }
}
