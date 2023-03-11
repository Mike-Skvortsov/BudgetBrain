using DataAccess.Migrations;
using DataAccess.Repositories;
using Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
	public class ColorService : IColorService
	{
		private readonly IColorRepository _repository;

		public ColorService(IColorRepository repository)
		{
			this._repository = repository;
		}

		public async Task<ICollection<ColorCard>> GetAllColorAsync()
		{
			return await this._repository.GetAllAsync();
		}
		public async Task<ColorCard> GetByColorValueAsync(string colorValue)
		{
			return await this._repository.GetByColorValueAsync(colorValue);
		}
	}
}
