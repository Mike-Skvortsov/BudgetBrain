using Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public interface IUserRepository
    {
        Task<ICollection<User>> GetAllAsync();
        Task<User> GetByIdAsync(int id);
        Task<User> GetUserByEmailAsync(string email);
        Task AddAsync(User user);
		Task UpdateAsync(User user);
        Task DeleteAsync(User user);
        Task<User> Search(string login, string passwordParam = null);
    }
}
