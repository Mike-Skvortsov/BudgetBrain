using Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public interface ICardRepository
    {
        Task<Card> GetByCard(int cardId);
		Task<Card> GetByCardNumber(string numberCard, int userId);
		Task<ICollection<Card>> GetAllAsync();
        Task<ICollection<Card>> GetByUserIdAsync(int userId);
		Task<Card> GetByIdAsync(int id, int userId);
        Task<decimal> GetBalanceUserAsync(int userId);
		Task AddAsync(Card car);
        Task UpdateAsync(Card car);
        Task DeleteAsync(Card car);
    }
}
