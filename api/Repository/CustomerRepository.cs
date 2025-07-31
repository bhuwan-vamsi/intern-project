using APIPractice.Data;
using APIPractice.Exceptions;
using APIPractice.Models.Domain;
using APIPractice.Models.DTO;
using APIPractice.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace APIPractice.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext db;

        public CustomerRepository(ApplicationDbContext db)
        {
            this.db = db;
        }
        public async Task<Customer> GetById(Guid id)
        {
            try
            {
                var customer = await db.Customers.FirstOrDefaultAsync(x => x.Id == id);
                if (customer == null)
                {
                    throw new Exception("User Not Found");
                }
                return customer;
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateAsync(Guid id, EditProfileRequest request)
        {
            try
            {
                var existingCustomer = await db.Customers.FirstOrDefaultAsync(u => u.Id == id);
                if (existingCustomer == null)
                {
                    throw new Exception("The user is not found");
                }
                if (db.Customers.Any(c => c.Phone == request.Phone && c.Id != id)) 
                {
                    throw new ConflictException("Phone Number Already Exists");
                }
                existingCustomer.Name = request.Name;
                existingCustomer.Address = request.Address;
                existingCustomer.Phone = request.Phone;
                db.Customers.Update(existingCustomer);
                await db.SaveChangesAsync();
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
              
        }
    }
}
