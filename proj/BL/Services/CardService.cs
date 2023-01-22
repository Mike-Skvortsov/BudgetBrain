using DataAccess.Repositories;
using Entities.Entities;
using System.Collections.Generic;
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

        public Task<ICollection<Card>> GetAllAsync()
        {
            var cardFromRepository = this._repository.GetAllAsync();
            return cardFromRepository;
        }

        public async Task<Card> GetByIdAsync(int id)
        {
            return await this._repository.GetByIdAsync(id);
        }

        public Task AddAsync(Card card)
            => this._repository.AddAsync(card);

        public Task UpdateAsync(Card card)
            => this._repository.UpdateAsync(card);

        public async Task<bool> TryUpdateAsync(int id, Card card)
        {
            var cardToUpdate = await this._repository.GetByIdAsync(id);
            if (cardToUpdate != null)
            {
                cardToUpdate.User = card.User;
                cardToUpdate.NumberCard = card.NumberCard;
                cardToUpdate.CardAmount = card.CardAmount;
                cardToUpdate.Id = card.Id;
                

                await this._repository.UpdateAsync(cardToUpdate);

                return true;
            }

            return false;
        }

        public Task DeleteAsync(Card card)
            => this._repository.DeleteAsync(card);
    }
}

