using Entities.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
	public class UserRepository : IUserRepository
	{
		private readonly DataBaseContext _context;
		public UserRepository(DataBaseContext context)
		{
			this._context = context;
		}
		public async Task<ICollection<User>> GetAllAsync()
		{
			return await this._context.Users.Include(x => x.Cards).ToListAsync();
		}
		//public async Task SaveTimeVerified(User user)
		//{
		//	user.VerifiedAt = DateTime.Now;
		//	await this._context.SaveChangesAsync();
		//}
		//public async Task<User> GetByToken(string token)
		//{
		//	return await this._context.Users.Include(x => x.Cards).FirstOrDefaultAsync(x => x.VerificationToken == token);
		//}

		public async Task<User> GetByIdAsync(int id)
		{
			return await this._context.Users.Include(x => x.Cards).FirstOrDefaultAsync(x => x.Id == id);
		}
		public async Task<User> GetUserByEmailAsync(string email)
		{
			return await this._context.Users.FirstOrDefaultAsync(x => x.Email == email);
		}

		public async Task<User> Search(string email, string passwordParam = null)
		{
			passwordParam = md5.HashPassword(passwordParam);
			return await this._context.Users.Include(x => x.Cards).FirstOrDefaultAsync(x => x.Email == email && x.Password == passwordParam);
		}
		public async Task AddAsync(User user)
		{
			user.Password = md5.HashPassword(user.Password);
			await this._context.Users.AddAsync(user);
			await this._context.SaveChangesAsync();
		}
		public async Task UpdateAsync(User user)
		{
			this._context.Users.Update(user);
			await this._context.SaveChangesAsync();
		}
		public async Task DeleteAsync(User user)
		{
			this._context.Users.Remove(user);
			await this._context.SaveChangesAsync();
		}
	}
}
