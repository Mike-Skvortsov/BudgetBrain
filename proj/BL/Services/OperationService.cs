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
		public async Task<ICollection<Operation>> GetAllAsync(int userId) 
            => await this._repository.GetAllAsync(userId);

		public Task<Operation> GetByIdAsync(int id)
            => this._repository.GetByIdAsync(id);
		public async Task<decimal> GetSumTypeOperation(OperationType operationType, int userId)
		{
			return await this._repository.GetSumTypeOperation(operationType, userId);
		}
		public async Task AddAsync(Operation operation, int userId, int cardId)
        {
            Card card = await _cardRepository.GetByIdAsync(cardId, userId);
            if (operation.Type == OperationType.Income)
            {
                card.CardAmount += operation.Sum;
            }
            if (operation.Type == OperationType.Expenses)
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
            var operationToUpdate = await this._repository.GetByIdAsync(id);
            var oldOperation = await _repository.GetByIdAsync(id);
            var card = await _cardRepository.GetByIdAsync(operation.CardId, operation.Card.UserId);
            if (oldOperation.Type == OperationType.Income)
            {
				card.CardAmount -= oldOperation.Sum;
			}
			else if (oldOperation.Type == OperationType.Expenses)
			{
				card.CardAmount += oldOperation.Sum;
			}
			if (operation.Type == OperationType.Income)
			{
                card.CardAmount += operation.Sum;
			}
			else if (operation.Type == OperationType.Expenses)
			{
				card.CardAmount -= operation.Sum;
			}
			if (operationToUpdate != null)
            {
				operationToUpdate.Sum = operation.Sum;
                operationToUpdate.Name = operation.Name;
                operationToUpdate.Category = operation.Category;
				operationToUpdate.Type = operation.Type;
                operationToUpdate.CreatedAt = operation.CreatedAt;
                await this._repository.UpdateAsync(operationToUpdate);

                return true;
            }

            return false;
        }

        public async Task DeleteAsync(Operation operation, int cardId, int userId)
        {

            var card = await _cardRepository.GetByIdAsync(cardId, userId);
            card.CardAmount -= operation.Sum;
            await this._repository.DeleteAsync(operation);
        }
    }
}

