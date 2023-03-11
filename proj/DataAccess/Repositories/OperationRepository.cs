using Entities.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
		public async Task<decimal> GetSumTypeOperation(OperationType operationType, int userId)
        {
			DateTime lastMonth = DateTime.Now.AddMonths(-1);
			return await this._context.Operations
				.Where(x => x.Card.UserId == userId && x.Type == operationType && x.CreatedAt >= lastMonth)
				.SumAsync(x => x.Sum);
		}


		public async Task<ICollection<Operation>> GetAllAsync(int userId)
        {
            return await this._context.Operations.Where(x => x.Card.UserId == userId).Include(x => x.Card).Include(x => x.Category).ToListAsync();
        }
		public async Task<Operation> GetByIdAsync(int id)
		{
			return await this._context.Operations.Include(x => x.Category).Include(x => x.Card).FirstOrDefaultAsync(x => x.Id == id);
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
