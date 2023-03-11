using Entities.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
	public class ColorRepository : IColorRepository
	{
		private readonly DataBaseContext _context;

		public ColorRepository(DataBaseContext context)
		{
			this._context = context;
		}

		public async Task<ICollection<ColorCard>> GetAllAsync()
		{
			return await this._context.ColorCards.ToListAsync();
		}
		public async Task<ColorCard> GetByColorValueAsync(string colorValue)
		{
			return await this._context.ColorCards.FirstOrDefaultAsync(x => x.Value == colorValue);
		}
	}
}
