using APIPractice.Models.Domain;
using APIPractice.Models.DTO;

namespace APIPractice.Services.IService
{
    public interface IAdminService
    {
        Task<List<Employee>> GetAllEmployee();
        Task<Employee> GetEmployee(Guid id);
        Task AssignManager(UpdateEmployeeRequest assignManagerRequest);
    }
}
