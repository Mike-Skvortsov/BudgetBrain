using Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public interface ICardService
    {
        Task<ICollection<Card>> GetAllAsync();
        Task<ICollection<Card>> GetByUserIdAsync(int userId);
		Task<Card> GetByIdAsync(int id, int userId);
        Task AddAsync(Card card);
        Task UpdateAsync(Card card);
        Task<bool> TryUpdateAsync(int id, Card card, int userId);
        Task DeleteAsync(Card card);
    }
}
