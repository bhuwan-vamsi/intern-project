using APIPractice.Models.Domain;
using APIPractice.Models.DTO;

namespace APIPractice.Repository.IRepository
{
    public interface ICustomerRepository
    {
        public Task<Customer> GetById(Guid id);
        public Task UpdateAsync(Guid id, EditProfileRequest request);
    }
}
