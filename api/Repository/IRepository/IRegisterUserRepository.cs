using APIPractice.Models.Domain;
using APIPractice.Models.DTO;
using Microsoft.AspNetCore.Identity;

namespace APIPractice.Repository.IRepository
{
    public interface IRegisterUserRepository
    {
        Task<Employee> AddEmployee(RegisterStaffRequest registerRequest, IdentityUser identityUser);
        Task<Manager> AddManager(RegisterStaffRequest registerRequest, IdentityUser identityUser);
        Task<Customer> AddCustomer(RegisterCustomerRequest registerRequest, IdentityUser identityUser);
    }
}
