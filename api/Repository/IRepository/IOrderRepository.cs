using APIPractice.Models.Domain;

namespace APIPractice.Repository.IRepository
{
    public interface IOrderRepository
    {
        public Task<Order> AddAsync(Order order);
        public Task UpdateAsync(Guid id, Order order);
        public Task<List<Order>> GetOrderHistoryOfCustomer(Guid customerId);
        public Task<Order?> GetOrderByIdAsync(Guid orderId, Guid customerId);
        public Task DeleteOrderOfCustomer(Guid customerId);

        public Task<List<Order>> GetOrdersByStatusAsync(string status);
        public Task<List<Order>> GetDeliveredOrdersByEmployeeAsync(Guid employeeId);
    }
}
