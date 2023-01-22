using AutoMapper;
using BL.Services;
using Entities.Entities;
using Microsoft.AspNetCore.Mvc;
using proj.Models;
using System.Security.Cryptography;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace proj.Controllers
{

    [ApiController]
    [Route("user")]
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

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] UserModel model)
        {
            var user = _mapper.Map<User>(model);
            var email = await _userService.GetUserByEmailAsync(user.Email);
			if (email == null)
			{
				await _userService.AddAsync(user);
				return Ok();
			}
			return this.Conflict();
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
