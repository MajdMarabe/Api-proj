using KASHOP.DAL.Data;
using KASHOP.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.Repository
{
    public class GenericRepository<T> : IGenericRepository <T> where T : class
    {
        private readonly ApplicationDbContext _context;
        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<T>> GetAllAsync(string[] ? includes = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (includes != null) {
            foreach( var include in includes)
                {
                   query = query.Include(include);
                }
            
            }
            return await query.ToListAsync();

           // return await _context.categories.Include(c => c.Translations).ToListAsync();
          //    return await _context.Set<T>().ToListAsync();

        }
        public async Task<T> CreateAsync(T item)
        {
            await _context.AddAsync(item);
            await _context.SaveChangesAsync();
            return item;
        }
        public async Task<T> GetOne( Expression<Func<T, bool>> filter,string[]? includes = null)
        {
            IQueryable<T> query = _context.Set<T>();


            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
                return await query.FirstOrDefaultAsync(filter);

        }

        public async Task<bool> DeleteAsync(T entity)
        {
             _context.Remove(entity);
          var affectred=  await _context.SaveChangesAsync();
            return affectred > 0;
        } 

        /* public async Task<T> GetByIdAsync(int id)
         {
            return  await _context.Set<T>().FindAsync(id);

         }*/
    }
}
