using Entities.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class CardRepository : ICardRepository
    {
        private readonly DataBaseContext _context;

        public CardRepository(DataBaseContext context)
        {
            this._context = context;
        }

        public async Task<ICollection<Card>> GetAllAsync()
        {
            return await this._context.Cards.Include(x => x.Operations).ThenInclude(o => o.Category).Include(x => x.ColorCard).OrderBy(x => x.CreatedAt).ToListAsync();
        }
		public async Task<decimal> GetBalanceUserAsync(int userId)
        {
			return await this._context.Cards.Where(x => x.UserId == userId).SumAsync(x => x.CardAmount);
		}
		public async Task<Card> GetByCard(int cardId)
		{
			return await this._context.Cards.Include(x => x.Operations).FirstOrDefaultAsync(x => x.Id == cardId);
		}

		public async Task<Card> GetByIdAsync(int id, int userId)
        {
            return await this._context.Cards.Include(x => x.Operations).ThenInclude(o => o.Category).Include(x => x.ColorCard).Where(x => x.UserId == userId).FirstOrDefaultAsync(x => x.Id == id);
        }
		public async Task<Card> GetByCardNumber(string cardNumber, int userId)
        {
            return await this._context.Cards.Where(x => x.UserId == userId).FirstOrDefaultAsync(x => x.NumberCard == cardNumber);
        }

		public async Task<ICollection<Card>> GetByUserIdAsync(int userId)
		{
			return await _context.Cards.Include(x => x.Operations).Include(x => x.ColorCard).Where(x => x.UserId == userId).ToListAsync(); 
		}
		public async Task AddAsync(Card card)
        {
            await this._context.Cards.AddAsync(card);
            await this._context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Card card)
        {
            this._context.Cards.Update(card);
            await this._context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Card card)
        {
            this._context.Cards.Remove(card);
            await this._context.SaveChangesAsync();
        }
    }
}
