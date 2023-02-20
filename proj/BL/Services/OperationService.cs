using DataAccess.Repositories;
using Entities.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BL.Services
{
    public class OperationService : IOperationService
    {
        private readonly IOperationRepository _repository;
        private readonly ICardRepository _cardRepository;

        public OperationService(IOperationRepository repository, ICardRepository cardRepository)
        {
            this._repository = repository;
            _cardRepository = cardRepository;
        }
        public Task<ICollection<Operation>> GetAllAsync()
        {
            var operationFromRepository = this._repository.GetAllAsync();
            return operationFromRepository;
        }

        public Task<Operation> GetByIdAsync(int id, int userId)
            => this._repository.GetByIdAsync(id, userId);

        public async Task AddAsync(Operation operation, int userId, int cardId)
        {
            Card card = await _cardRepository.GetByIdAsync(cardId, userId);
            if ((operation.Type == OperationType.Enrollment && operation.Sum >= 0) || operation.Type == OperationType.WritingOff && operation.Sum < 0)
            {
                card.CardAmount += operation.Sum;
            }
            if (operation.Type == OperationType.WritingOff && operation.Sum >= 0)
            {
                card.CardAmount -= operation.Sum;
            }
            operation.Card = card;
            await this._repository.AddAsync(operation);
        }

        public Task UpdateAsync(Operation operation)
            => this._repository.UpdateAsync(operation);

        public async Task<bool> TryUpdateAsync(int id, Operation operation)
        {
            //var operationToUpdate = await this._repository.GetByIdAsync(id);
            //var oldOperation = await _repository.GetByIdAsync(id);
            //var card = await _cardRepository.GetByIdAsync(operation.CardId);
            //if (card == null)
            //{
            //    return false;
            //}
            //card.CardAmount -= oldOperation.Sum;
            //card.CardAmount += operation.Sum;
            //if (operationToUpdate != null)
            //{
            //    operationToUpdate.Sum = operation.Sum;
            //    operationToUpdate.Name = operation.Name;
            //    operationToUpdate.Category = operation.Category;
            //    operationToUpdate.CardId = operation.CardId;
            //    operationToUpdate.Type = operation.Type;
            //    await this._repository.UpdateAsync(operationToUpdate);

            //    return true;
            //}

            return false;
        }

        public async Task DeleteAsync(Operation operation)
        {

            //var card = await _cardRepository.GetByIdAsync(operation.CardId);
            //card.CardAmount -= operation.Sum;
            //await this._repository.DeleteAsync(operation);
        }
    }
}

