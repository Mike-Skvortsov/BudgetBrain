using Azure.Storage.Blobs;
using DataAccess;
using DataAccess.Repositories;
using Entities.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly BlobServiceClient _blobService;

        public UserService(IUserRepository repository, BlobServiceClient blobService)
        {
            this._repository = repository;
            this._blobService = blobService;
		}
		    public async Task<string> SavePhotoAsync(int userId, byte[] photo)
		    {
			    var blobName = $"{userId}_{DateTime.UtcNow:yyyyMMddHHmmssfff}.jpg";
			    // Upload the photo to blob storage and get the URL
			    var containerName = "blobstorageimage1";
			    var containerClient = _blobService.GetBlobContainerClient(containerName);
			    var blobClient = containerClient.GetBlobClient(blobName);
			    await blobClient.UploadAsync(new MemoryStream(photo), overwrite: true);

			    return blobClient.Uri.ToString();
		    }

		public Task<ICollection<User>> GetAllAsync()
        {
            var userFromRepository = this._repository.GetAllAsync();
            return userFromRepository;
        }
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
                userToUpdate.Photo = user.Photo;

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
