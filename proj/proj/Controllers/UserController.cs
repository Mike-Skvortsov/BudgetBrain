using AutoMapper;
using BL.Services;
using Entities.Entities;
using Microsoft.AspNetCore.Mvc;
using proj.Models;
using System.Threading.Tasks;

namespace proj.Controllers
{
	[Route("api/user")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly IUserService _userService;
		private readonly IMapper _mapper;

		public UserController(IUserService userService, IMapper mapper)
		{
			this._userService = userService;
			this._mapper = mapper;
		}
		[HttpGet]
		public async Task<IActionResult> GetAllAsync()
		{
			var users = await this._userService.GetAllAsync();
			return this.Ok(users);
		}
		[HttpGet("{id:int}")]
		public async Task<IActionResult> GetById(int id)
		{
			var user = await this._userService.GetByIdAsync(id);
			if (user == null)
			{
				return this.NotFound();
			}
			return this.Ok(user);
		}

		[HttpPut("{id:int}/edit")]
		public async Task<IActionResult> UpdateAsync([FromBody] UserModel model, [FromRoute] int id)
		{
			var user = this._mapper.Map<User>(model);
			var result = await this._userService.TryUpdateAsync(user, id);
			if (result)
			{
				return this.Ok();
			}
			else
			{
				return this.NotFound();
			}
		}

		[HttpDelete("{id:int}/delete")]
		public async Task<IActionResult> DeleteAsync([FromRoute] int id)
		{
			var user = await this._userService.GetByIdAsync(id);
			if (user != null)
			{
				await this._userService.DeleteAsync(user);

				return this.Ok();
			}

			return this.NotFound();
		}
	}
}
