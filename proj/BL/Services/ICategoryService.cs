using Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
	public interface ICategoryService
	{
		Task<ICollection<Category>> GetAllCategoryAsync();
		Task<Category> GetByCategoryNameAsync(string categoryName, OperationType operationType);
		Task<ICollection<Category>> GetByCategory(OperationType typeCategory);
	}
}
