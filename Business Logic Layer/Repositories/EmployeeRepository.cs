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
    public class EmployeeRepository : GeneircRepository<Employee>, IEmployeeRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public EmployeeRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<Employee> GetEmployeesByAddress(string address)
            => _dbContext.Employees.Where(x => x.Address == address);


    }
}
