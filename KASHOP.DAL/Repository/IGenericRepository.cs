using KASHOP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        public  Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter = null, string[]? includes = null);
        public Task<T> CreateAsync(T item);
        public Task<T> GetOne(Expression<Func<T, bool>> filter, string[]? includes = null);
        public Task<bool> DeleteAsync(T entity);
        public Task<bool> UpdateAsync(T entity);


    }
}
