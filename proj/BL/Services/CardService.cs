using DataAccess.Repositories;
using Entities.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BL.Services
{
    public class CardService : ICardService
    {
        private readonly ICardRepository _repository;

        public CardService(ICardRepository repository)
        {
            this._repository = repository;
        }

        public async Task<ICollection<Card>> GetAllAsync()
        {
            var cardFromRepository = await this._repository.GetAllAsync();
			return cardFromRepository.ToList();
		}
		public async Task<ICollection<Card>> GetByUserIdAsync(int userId)
		{
			var cardFromRepository = await this._repository.GetByUserIdAsync(userId);
			return cardFromRepository.ToList();
		}

		public async Task<Card> GetByIdAsync(int id, int userId)
        {
            return await this._repository.GetByIdAsync(id, userId);
        }

        public Task AddAsync(Card card)
            => this._repository.AddAsync(card);

        public Task UpdateAsync(Card card)
            => this._repository.UpdateAsync(card);

        public async Task<bool> TryUpdateAsync(int id, Card card, int userId)
        {
            var cardToUpdate = await this._repository.GetByIdAsync(id, userId);
            if (cardToUpdate != null)
            {
                cardToUpdate.User = card.User;
                cardToUpdate.NumberCard = card.NumberCard;
                cardToUpdate.CardAmount = card.CardAmount;
                cardToUpdate.CardName = card.CardName;

                await this._repository.UpdateAsync(cardToUpdate);

                return true;
            }

            return false;
        }

        public Task DeleteAsync(Card card)
            => this._repository.DeleteAsync(card);
    }
}

