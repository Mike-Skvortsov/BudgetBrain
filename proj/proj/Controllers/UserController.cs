using AutoMapper;
using BL.Services;
using Entities.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using proj.Models;
using proj.Models.DTO;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace proj.Controllers
{
	[Route("api/user")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly IUserService _userService;
		private readonly ICardService _cardService;
		private readonly IOperationService _operationService;
		private readonly IMapper _mapper;
		private int userId;

		public UserController(IUserService userService, IMapper mapper, ICardService cardService, IOperationService operationService)
		{
			this._userService = userService;
			this._mapper = mapper;
			this._cardService = cardService;
			this._operationService = operationService;
		}
		[Authorize]
		[HttpGet("balance")]
		public async Task<IActionResult> GetSumType()
		{
			var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
			if (userIdClaim == null)
			{
				return null;
			}
			userId = int.Parse(userIdClaim.Value);

			var balance = await _cardService.GetBalanceUserAsync(userId);
			var income = await _operationService.GetSumTypeOperation(OperationType.Income, userId);
			var expenses = await _operationService.GetSumTypeOperation(OperationType.Expenses, userId);
			var result = new
			{
				Balance = balance,
				Income = income,
				Expenses = expenses,
				Total = income - expenses
			};

			return Ok(result);
		}
		[HttpGet("all")]
		public async Task<IActionResult> GetAllAsync()
		{
			var users = await this._userService.GetAllAsync();
			var model = this._mapper.Map<ICollection<UserModel>>(users);

			return this.Ok(model);
		}
		[Authorize]
		[HttpGet("")]
		public async Task<IActionResult> GetById()
		{
			var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
			if (userIdClaim == null)
			{
				return null;
			}
			userId = int.Parse(userIdClaim.Value);
			var user = await _userService.GetByIdAsync(userId);
			var model = this._mapper.Map<UserGetID>(user);

			return Ok(model);
		}

		[HttpPut("{id:int}/edit")]
		public async Task<IActionResult> UpdateAsync([FromBody] UserUpdate model, [FromRoute] int id)
		{
			var user = this._mapper.Map<User>(model);
			if (!string.IsNullOrEmpty(model.Photo))
			{
				byte[] photoBytes = Convert.FromBase64String(model.Photo);

				var photoUrl = await _userService.SavePhotoAsync(id, photoBytes);
				user.Photo = photoUrl;
			}

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

		[HttpDelete("delete")]
		public async Task<IActionResult> DeleteAsync()
		{
			var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
			if (userIdClaim == null)
			{
				return null;
			}
			userId = int.Parse(userIdClaim.Value);
			var user = await _userService.GetByIdAsync(userId);
			if (user == null)
			{
				return this.NotFound();
			}
			await this._userService.DeleteAsync(user);
			return this.Ok();
		}
	}
}
