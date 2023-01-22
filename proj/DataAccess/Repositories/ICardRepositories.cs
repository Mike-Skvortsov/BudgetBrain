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
        Task<ICollection<Card>> GetAllAsync();
        Task<Card> GetByIdAsync(int id);
        Task AddAsync(Card car);
        Task UpdateAsync(Card car);
        Task DeleteAsync(Card car);
    }
}
