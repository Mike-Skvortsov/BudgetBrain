using DataAccess.Migrations;
using Entities.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BL.Services
{
    public interface IOperationService
    {
		Task<decimal> GetSumTypeOperation(OperationType operationType, int userId);
		Task<ICollection<Operation>> GetAllAsync(int userId);
        Task<Operation> GetByIdAsync(int id);
        Task AddAsync(Operation operation, int userId, int cardId);
        Task UpdateAsync(Operation operation);
        Task<bool> TryUpdateAsync(int id, Operation operation);
        Task DeleteAsync(Operation operation, int cardId, int userId);
    }
}
