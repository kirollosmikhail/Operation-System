using Business_Logic_Layer.Interfaces;
using Data_Access_Layer.Contexts;
using Data_Access_Layer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic_Layer.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public DepartmentRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int Add(Department department)
        {

            _dbContext.Departments.Add(department);
            return _dbContext.SaveChanges();

        }

        public int Delete(Department department)
        {
            _dbContext.Remove(department);
            return _dbContext.SaveChanges();
        }

        public IEnumerable<Department> GetAll()
        => _dbContext.Departments.ToList();


        public Department GetById(int id)
        {
            return _dbContext.Departments.Find(id);

        }

        public int Update(Department department)
        {
            _dbContext.Departments.Update(department);
            return _dbContext.SaveChanges();
        }
    }
}
