using APIPractice.Data;
using APIPractice.Models.Domain;
using APIPractice.Models.DTO;
using APIPractice.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace APIPractice.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext db;

        public EmployeeRepository(ApplicationDbContext db) 
        {
            this.db = db;
        }
        public async Task DeleteEmployeeAsync(Guid id)
        {
            var employee = await  db.Employees.FirstOrDefaultAsync(u => u.Id==id);
            if (employee == null)
            {
                throw new KeyNotFoundException("Employee Not Found");
            }
            employee.IsActive = false;
        }

        public async Task<List<Employee>> GetAllEmployeesAsync()
        {
            return await db.Employees.Include("Manager").Where(x => x.IsActive == true).ToListAsync();
        }

        public async Task<Employee> GetEmployeeByIdAsync(Guid id)
        {
            var employee = await db.Employees.Include("Manager").FirstOrDefaultAsync(x=> x.Id == id);
            if (employee == null)
            {
                throw new KeyNotFoundException("Employee Not Found");
            }
            return employee;
        }

        public async Task UpdateEmployeeAsync(UpdateEmployeeRequest updateEmployee)
        {
            var employee = await db.Employees.FirstOrDefaultAsync(u => u.Id == updateEmployee.EmployeeId);
            if (employee == null)
            {
                throw new KeyNotFoundException("Employee Not Found");
            }
            if(updateEmployee.ManagerId == null)
            {
                employee.ManagerId = null;
            }
            else
            {
                var manager = await db.Managers.FirstOrDefaultAsync(x => x.Id == updateEmployee.ManagerId);
                if (manager == null)
                {
                    throw new KeyNotFoundException("Manager Not Found");
                }
                employee.ManagerId = updateEmployee.ManagerId;
            }
            db.Employees.Update(employee);
            await db.SaveChangesAsync();
        }
    }
}
