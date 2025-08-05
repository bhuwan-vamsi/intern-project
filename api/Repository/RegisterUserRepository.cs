using APIPractice.Data;
using APIPractice.Models.Domain;
using APIPractice.Models.DTO;
using APIPractice.Repository.IRepository;
using Microsoft.AspNetCore.Identity;

namespace APIPractice.Repository
{
    public class RegisterUserRepository : IRegisterUserRepository
    {
        private readonly ApplicationDbContext db;

        public RegisterUserRepository(ApplicationDbContext db) 
        {
            this.db = db;
        }
        public async Task<Employee> AddEmployee(RegisterStaffRequest registerRequest, IdentityUser identityUser)
        {
            var user = new Employee
            {
                Id = Guid.Parse(identityUser.Id),
                IsActive = true
            };
            await db.Employees.AddAsync(user);
            await db.SaveChangesAsync();
            return user;
        }
        public async Task<Manager> AddManager(RegisterStaffRequest registerRequest, IdentityUser identityUser)
        {
            var user = new Manager
            {
                Id = Guid.Parse(identityUser.Id),
                IsActive = true
            };
            await db.Managers.AddAsync(user);
            await db.SaveChangesAsync();
            return user;
        }

        public async Task<Customer> AddCustomer(RegisterCustomerRequest registerCustomer, IdentityUser identityUser)
        {
            try
            {
                var user = new Customer
                {
                    Id = Guid.Parse(identityUser.Id),
                    IsActive = true,
                    PhoneNumber = registerCustomer.PhoneNumber,
                };

                await db.Customers.AddAsync(user);
                await db.SaveChangesAsync();
                return user;
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
