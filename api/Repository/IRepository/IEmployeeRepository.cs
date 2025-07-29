using APIPractice.Models.Domain;
using APIPractice.Models.DTO;

namespace APIPractice.Repository.IRepository
{
    public interface IEmployeeRepository
    {
        Task<Employee> GetEmployeeByIdAsync(Guid id);
        Task<List<Employee>> GetAllEmployeesAsync();
        Task UpdateEmployeeAsync(UpdateEmployeeRequest employee);
        Task DeleteEmployeeAsync(Guid id);
    }
}
