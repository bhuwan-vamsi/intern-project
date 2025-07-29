using APIPractice.Models.Domain;
using APIPractice.Models.DTO;

namespace APIPractice.Services.IService
{
    public interface IAdminService
    {
        Task RegisterEmployee(RegisterEmployeeRequest registerCustomerRequest);
        Task AssignManager(UpdateEmployeeRequest assignManagerRequest);
        Task<List<Employee>> GetAllEmployee();
        Task<Employee> GetEmployee(Guid id);
    }
}
