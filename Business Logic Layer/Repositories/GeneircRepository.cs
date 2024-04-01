using Business_Logic_Layer.Interfaces;
using Data_Access_Layer.Contexts;
using Data_Access_Layer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic_Layer.Repositories
{
    public class GeneircRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _dbcontext;

        public GeneircRepository(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task AddAsync(T item)
        {
            await _dbcontext.AddAsync(item);
        }

        public void Delete(T item)
        {
            _dbcontext.Remove(item);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            if (typeof(T) == typeof(Employee))
            {
                return  (IEnumerable<T>) await _dbcontext.Employees.Include(x => x.Department).ToListAsync();
            }
            return await _dbcontext.Set<T>().ToListAsync();
        }


        public async Task<T> GetByIdAsync(int id)
            => await _dbcontext.Set<T>().FindAsync(id);


        public void Update(T item)
        {
            _dbcontext.Update(item);
        }
    }
}
