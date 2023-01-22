using Entities.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class OperationRepository : IOperationRepository
    {
        private readonly DataBaseContext _context;

        public OperationRepository(DataBaseContext context)
        {
            this._context = context;
        }

        public async Task<ICollection<Operation>> GetAllAsync()
        {
            return await this._context.Operations.Include(x => x.Card).ToListAsync();
        }

        public async Task<Operation> GetByIdAsync(int id)
        {
            return await this._context.Operations.Include(x => x.Card).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddAsync(Operation operation)
        {
            await this._context.Operations.AddAsync(operation);
            await this._context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Operation operation)
        {
            this._context.Operations.Update(operation);
            await this._context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Operation operation)
        {
            this._context.Operations.Remove(operation);
            await this._context.SaveChangesAsync();
        }
    }
}
