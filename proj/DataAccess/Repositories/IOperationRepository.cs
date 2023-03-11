using Entities.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public interface IOperationRepository
    {
        Task<decimal> GetSumTypeOperation(OperationType operationType, int userId);
		Task<ICollection<Operation>> GetAllAsync(int userId);
        Task<Operation> GetByIdAsync(int id);
        Task AddAsync(Operation operation);
        Task UpdateAsync(Operation operation);
        Task DeleteAsync(Operation operation);
    }
}
