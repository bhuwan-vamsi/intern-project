using APIPractice.Data;
using Microsoft.EntityFrameworkCore;

namespace APIPractice.Infrastructure
{
    public class TransactionManager
    {
        private readonly ApplicationDbContext db;

        public TransactionManager(ApplicationDbContext db)
        {
            this.db = db;
        }
        public async Task ExecuteInTransactionAsync(Func<Task> action)
        {
            using var transaction = await db.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);
            try
            {
                await action();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
