using DataAccess;
using DataAccess.Repositories;
using Entities.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            this._repository = repository;
        }

		//public  Task SaveTimeVerified(User user)
  //      {
		//	return this._repository.SaveTimeVerified(user);
		//}

		public Task<ICollection<User>> GetAllAsync()
        {
            var userFromRepository = this._repository.GetAllAsync();
            return userFromRepository;
        }
		//public async Task<User> GetByToken(string token)
		//	=> await _repository.GetByToken(token);

		public async Task<User> GetByIdAsync(int id) 
            => await _repository.GetByIdAsync(id);
		public async Task<User> GetUserByEmailAsync(string email)
			=> await _repository.GetUserByEmailAsync(email);

		public Task AddAsync(User user)
		{
			return this._repository.AddAsync(user);
		}

		public Task UpdateAsync(User user)
            => this._repository.UpdateAsync(user);

        public async Task<bool> TryUpdateAsync(User user, int id)
        {
            var userToUpdate = await this._repository.GetByIdAsync(id);
            if (userToUpdate != null)
            {
                userToUpdate.Cards = user.Cards;
                userToUpdate.FirstName = user.FirstName;
                userToUpdate.LastName = user.LastName;
				userToUpdate.Email = user.Email;
				userToUpdate.Password = user.Password;

				await this._repository.UpdateAsync(userToUpdate);

                return true;
            }

            return false;
        }

        public Task DeleteAsync(User user)
            => this._repository.DeleteAsync(user);
        public async Task<User> Search(string login, string passwordParam = null)
            => await this._repository.Search(login, passwordParam);
    }
}
