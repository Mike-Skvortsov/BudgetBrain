using Entities.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BL.Services
{
    public interface IOperationService
    {
        Task<ICollection<Operation>> GetAllAsync();
        Task<Operation> GetByIdAsync(int id);
        Task AddAsync(Operation operation);
        Task UpdateAsync(Operation operation);
        Task<bool> TryUpdateAsync(int id, Operation operation);
        Task DeleteAsync(Operation operation);
    }
}
