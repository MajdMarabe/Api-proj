using KASHOP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.Repository
{
    public interface IGenericRepositery<T> where T : class
    {
        public Task<List<T>> GetAllAsync(string[]? includes = null);
        public Task<T> CreateAsync(T item);
        public Task<T> GetOne(string[]? includes, Expression<Func<T, bool>> filter);

      //  public  Task<T> GetByIdAsync(T item);

    }
}
