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
    public class DepartmentRepository : GeneircRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(ApplicationDbContext dbContext):base(dbContext)
        {
            
        }
    }
}
