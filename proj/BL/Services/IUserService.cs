using Entities.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public interface IUserService
    {
		Task<string> SavePhotoAsync(int userId, byte[] photo);
		Task<ICollection<User>> GetAllAsync();
        Task<User> GetByIdAsync(int id);
        Task<User> GetUserByEmailAsync(string email);
        Task AddAsync(User user);
		Task UpdateAsync(User user);
        Task<bool> TryUpdateAsync(User user,int id);
        Task DeleteAsync(User user);
		Task<User> Search(string login, string passwordParam = null);
	}
}
