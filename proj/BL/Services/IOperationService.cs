using DataAccess.Migrations;
using Entities.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BL.Services
{
    public interface IOperationService
    {
        Task<ICollection<Operation>> GetAllAsync();
        Task<Operation> GetByIdAsync(int id, int userId);
        Task AddAsync(Operation operation, int userId, int cardId);
        Task UpdateAsync(Operation operation);
        Task<bool> TryUpdateAsync(int id, Operation operation);
        Task DeleteAsync(Operation operation);
    }
}
