using Entities.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public interface IOperationRepository
    {
        Task<ICollection<Operation>> GetAllAsync();
        Task<Operation> GetByIdAsync(int id);
        Task AddAsync(Operation operation);
        Task UpdateAsync(Operation operation);
        Task DeleteAsync(Operation operation);
    }
}
