using DataAccess.Repositories;
using Entities.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BL.Services
{
	public class CategoryService : ICategoryService
	{
		private readonly ICategoryRepository _repository;

		public CategoryService(ICategoryRepository repository)
		{
			this._repository = repository;
		}
		public async Task<ICollection<Category>> GetAllCategoryAsync()
		{
			return await this._repository.GetAllCategoryAsync();
		}
		public async Task<Category> GetByCategoryNameAsync(string categoryName, OperationType operationType)
		{
			return await this._repository.GetByCategoryNameAsync(categoryName, operationType);
		}
		public async Task<ICollection<Category>> GetByCategory(OperationType typeCategory)
		{
			return await this._repository.GetByCategory(typeCategory);
		}
	}
}
