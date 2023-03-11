using Entities.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
	public class CategoryRepository: ICategoryRepository
	{
		private readonly DataBaseContext _context;

		public CategoryRepository(DataBaseContext context)
		{
			this._context = context;
		}

		public async Task<ICollection<Category>> GetAllCategoryAsync()
		{
			return await this._context.Categories.ToListAsync();
		}
		public async Task<Category> GetByCategoryNameAsync(string categoryName, OperationType operationType)
		{
			return await this._context.Categories.FirstOrDefaultAsync(x => x.Name == categoryName && x.TypeCategory == operationType);
		}
		public async Task<ICollection<Category>> GetByCategory(OperationType typeCategory)
		{
			return await this._context.Categories.Where(x => x.TypeCategory == typeCategory).ToListAsync();
		}
	}
}
