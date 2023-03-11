using Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
	public interface IColorRepository
	{
		Task<ICollection<ColorCard>> GetAllAsync();
		Task<ColorCard> GetByColorValueAsync(string colorValue);
	}
}
