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
	[Authorize]
	[ApiController]
	[Route("api/operation")]
	public class OperationController : ControllerBase
	{
		private readonly IOperationService _operationService;
		private readonly IMapper _mapper;
		private readonly ICardService _cardService;
		private readonly IUserService _userService;
		private readonly ICategoryService _categoryService;
		private int userId;

		public OperationController(IOperationService operationService, IMapper mapper, IUserService userService, ICategoryService categoryService, ICardService cardService)
		{
			this._operationService = operationService;
			this._mapper = mapper;
			this._userService = userService;
			this._categoryService = categoryService;
			_cardService = cardService;
		}
		private async Task<User> GetUserAsync()
		{
			var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
			if (userIdClaim == null)
			{
				return null;
			}
			userId = int.Parse(userIdClaim.Value);
			var user = await _userService.GetByIdAsync(userId);
			return user;
		}
		[HttpGet()]
		public async Task<IActionResult> GetAllOperations()
		{
			var user = await GetUserAsync();
			if (user == null)
			{
				return Unauthorized();
			}
			var operations = await _operationService.GetAllAsync(user.Id);
			var model = this._mapper.Map<ICollection<OperationModel>>(operations);

			return this.Ok(model);
		}
		[HttpGet("{id:int}")]
		public async Task<IActionResult> GetByIdOperations(int id)
		{
			var user = await GetUserAsync();
			if (user == null)
			{
				return Unauthorized();
			}
			var operation = await this._operationService.GetByIdAsync(id);
			if (operation == null)
			{
				return this.NotFound();
			}

			var model = this._mapper.Map<OperationDTOOneOperation>(operation);
			model.CardName = operation.Card.CardName;
			return this.Ok(model);
		}
		[HttpPost("create")]
		public async Task<IActionResult> Create([FromBody] OperationDTO1 model)
		{
			var user = await GetUserAsync();
			if (user == null)
			{
				return Unauthorized();
			}
			var category = await this._categoryService.GetByCategoryNameAsync(model.CategoryName, model.Type);
			var operation = this._mapper.Map<Operation>(model);
			operation.Category = category;
			if(operation.Sum < 0)
			{
				throw new Exception("The sum cannot be less than zero");
			}
			var card = await _cardService.GetByCard(operation.CardId);
			if(operation.Type == OperationType.Expenses && card.CardAmount - operation.Sum < 0)
			{
				return BadRequest("The balance on the card cannot be negative");
			}
			await _operationService.AddAsync(operation, user.Id, operation.CardId);
			return this.Ok();
		}

		[HttpPut("{id:int}/edit")]
		public async Task<IActionResult> Update([FromBody] OperationDTO1 model, [FromRoute] int id)
		{
			var user = await GetUserAsync();
			if (user == null)
			{
				return Unauthorized();
			}
			var operation = this._mapper.Map<Operation>(model);
			operation.Category = await this._categoryService.GetByCategoryNameAsync(model.CategoryName, model.Type);
			if (operation.Sum < 0)
			{
				throw new Exception("The sum cannot be less than zero");
			}
			if (operation.Type == OperationType.Expenses && operation.Card.CardAmount - operation.Sum < 0)
			{
				throw new Exception("The balance on the card cannot be negative");
			}
			var result = await this._operationService.TryUpdateAsync(id, operation);
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
			var user = await GetUserAsync();
			if (user == null)
			{
				return Unauthorized();
			}
			var operation = await this._operationService.GetByIdAsync(id);
			if (operation != null)
			{
				await this._operationService.DeleteAsync(operation, operation.CardId, user.Id);
				return this.Ok();
			}

			return this.NotFound();
		}
	}
}
