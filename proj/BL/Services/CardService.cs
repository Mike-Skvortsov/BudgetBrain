using DataAccess.Migrations;
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
		public async Task<Card> GetByCard(int cardId)
        {
			return await this._repository.GetByCard(cardId);
		}

		public async Task<decimal> GetBalanceUserAsync(int userId)
        {
			return await this._repository.GetBalanceUserAsync(userId);
		}

		public async Task<ICollection<Card>> GetAllAsync()
        {
            var cards = await this._repository.GetAllAsync();
			return cards.ToList();
		}
		public async Task<ICollection<Card>> GetByUserIdAsync(int userId)
		{
			var cards = await this._repository.GetByUserIdAsync(userId);
			return cards.ToList();
		}
		public async Task<Card> GetByCardNumber(string cardNumber, int userId)
			=> await _repository.GetByCardNumber(cardNumber, userId);

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
                cardToUpdate.NumberCard = card.NumberCard;
                cardToUpdate.CardAmount = card.CardAmount;
                cardToUpdate.CardName = card.CardName;
                cardToUpdate.ColorCard = card.ColorCard;
                cardToUpdate.ColorId = card.ColorCard.Id;

                await this._repository.UpdateAsync(cardToUpdate);

                return true;
            }

            return false;
        }

        public Task DeleteAsync(Card card)
            => this._repository.DeleteAsync(card);
    }
}

