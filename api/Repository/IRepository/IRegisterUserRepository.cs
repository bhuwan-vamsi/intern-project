using APIPractice.Models.Domain;
using APIPractice.Models.DTO;
using Microsoft.AspNetCore.Identity;

namespace APIPractice.Repository.IRepository
{
    public interface IRegisterUserRepository
    {
        Task<Employee> AddEmployee(RegisterEmployeeRequest registerRequest, IdentityUser identityUser);
        Task<Manager> AddManager(RegisterEmployeeRequest registerRequest, IdentityUser identityUser);
        Task<Customer> AddCustomer(RegisterCustomerRequest registerRequest, IdentityUser identityUser);
    }
}
