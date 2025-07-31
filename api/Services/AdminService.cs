using APIPractice.Models.Domain;
using APIPractice.Models.DTO;
using APIPractice.Repository;
using APIPractice.Repository.IRepository;
using APIPractice.Services.IService;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;

namespace APIPractice.Services
{
    public class AdminService : IAdminService
    {
        private readonly Microsoft.AspNetCore.Identity.UserManager<IdentityUser> userManager;
        private readonly IRegisterUserRepository userRepository;
        private readonly IEmployeeRepository employeeRepository;

        public AdminService( Microsoft.AspNetCore.Identity.UserManager<IdentityUser> userManager, IRegisterUserRepository userRepository, IEmployeeRepository employeeRepository)
        {
            this.userManager = userManager;
            this.userRepository = userRepository;
            this.employeeRepository = employeeRepository;
        }

        public async Task RegisterEmployee(RegisterEmployeeRequest registerEmployeeRequest)
        {
            var identityUser = new IdentityUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = registerEmployeeRequest.UserName,
                Email = registerEmployeeRequest.UserName
            };
            var identityResult = await userManager.CreateAsync(identityUser, registerEmployeeRequest.Password);
            if (identityResult.Succeeded)
            {
                if (registerEmployeeRequest.Role != null && (registerEmployeeRequest.Role.ToLower().Equals("manager") || registerEmployeeRequest.Role.ToLower().Equals("employee")))
                {
                    identityResult = await userManager.AddToRoleAsync(identityUser, registerEmployeeRequest.Role);
                    if (identityResult.Succeeded)
                    {
                        if (registerEmployeeRequest.Role.ToLower() == "manager")
                        {
                            await userRepository.AddManager(registerEmployeeRequest, identityUser);
                        }
                        else if (registerEmployeeRequest.Role.ToLower() == "employee")
                        {
                            await userRepository.AddEmployee(registerEmployeeRequest, identityUser);
                        }
                    }
                    else
                    {
                        throw new Exception("Invalid Role");
                    }
                }
                else
                {
                    throw new Exception("Role not specified");
                }
            }
            else
            {
                throw new Exception("Username already exists");
            }
        }


        public async Task AssignManager(UpdateEmployeeRequest assignManagerRequest)
        {
            await employeeRepository.UpdateEmployeeAsync(assignManagerRequest);
        }

        public async Task<List<Employee>> GetAllEmployee()
        {
            return await employeeRepository.GetAllEmployeesAsync();
        }

        public async Task<Employee> GetEmployee(Guid id)
        {
            return await employeeRepository.GetEmployeeByIdAsync(id);
        }
    }
}
