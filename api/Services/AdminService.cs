using APIPractice.Models.Domain;
using APIPractice.Models.DTO;
using APIPractice.Repository.IRepository;
using APIPractice.Services.IService;

namespace APIPractice.Services
{
    public class AdminService : IAdminService
    {
        private readonly IEmployeeRepository employeeRepository;

        public AdminService(IEmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }

        public async Task<List<Employee>> GetAllEmployee()
        {
            return await employeeRepository.GetAllEmployeesAsync();
        }

        public async Task<Employee> GetEmployee(Guid id)
        {
            return await employeeRepository.GetEmployeeByIdAsync(id);
        }

        public async Task AssignManager(UpdateEmployeeRequest assignManagerRequest)
        {
            await employeeRepository.UpdateEmployeeAsync(assignManagerRequest);
        }
    }
}
