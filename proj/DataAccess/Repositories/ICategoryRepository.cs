using Entities.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
	public interface ICategoryRepository
	{
		Task<ICollection<Category>> GetAllCategoryAsync();
		Task<Category> GetByCategoryNameAsync(string categoryName, OperationType operationType);
		Task<ICollection<Category>> GetByCategory(OperationType typeCategory);
	}
}
