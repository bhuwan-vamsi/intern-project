using APIPractice.Data;
using APIPractice.Models.Domain;
using APIPractice.Models.DTO;
using APIPractice.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace APIPractice.Repository
{
    public class RegisterUserRepository : IRegisterUserRepository
    {
        private readonly ApplicationDbContext db;

        public RegisterUserRepository(ApplicationDbContext db) 
        {
            this.db = db;
        }
        public async Task<Employee> AddEmployee(RegisterEmployeeRequest registerRequest, IdentityUser identityUser)
        {
            var user = new Employee
            {
                Id = Guid.Parse(identityUser.Id),
                Name = registerRequest.Name,
                IsActive = true
            };
            await db.Employees.AddAsync(user);
            await db.SaveChangesAsync();
            return user;
        }
        public async Task<Manager> AddManager(RegisterEmployeeRequest registerRequest, IdentityUser identityUser)
        {
            var user = new Manager
            {
                Id = Guid.Parse(identityUser.Id),
                Name = registerRequest.Name
            };
            await db.Managers.AddAsync(user);
            await db.SaveChangesAsync();
            return user;
        }

        public async Task<Customer> AddCustomer(RegisterCustomerRequest registerCustomer, IdentityUser identityUser)
        {
            var user = new Customer
            {
                Id = Guid.Parse(identityUser.Id),
                Name = registerCustomer.Name,
                IsActive = true,
                Phone = registerCustomer.Phone,
            };

            await db.Customers.AddAsync(user);
            await db.SaveChangesAsync();
            return user;
        }
    }
}
