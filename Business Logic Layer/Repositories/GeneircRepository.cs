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

        public void Add(T item)
        {
            _dbcontext.Add(item);
        }

        public void Delete(T item)
        {
            _dbcontext.Remove(item);
        }

        public IEnumerable<T> GetAll()
        {
            if (typeof(T) == typeof(Employee))
            {
                return (IEnumerable<T>) _dbcontext.Employees.Include(x => x.Department).ToList();
            }
            return _dbcontext.Set<T>().ToList();
        }


        public T GetById(int id)
            => _dbcontext.Set<T>().Find(id);


        public void Update(T item)
        {
            _dbcontext.Update(item);
        }
    }
}
