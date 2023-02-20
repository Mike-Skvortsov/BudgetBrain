using AutoMapper;
using BL.Services;
using Entities.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using proj.Models;
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
		private readonly ICardService _cardService;
		private readonly IMapper _mapper;
		private readonly IUserService _userService;
		private int userId;

		public OperationController(IOperationService operationService, ICardService cardService, IMapper mapper, IUserService userService)
		{
			this._operationService = operationService;
			this._cardService = cardService;
			this._mapper = mapper;
			this._userService = userService;

		}
		//[HttpGet]
		//public async Task<IActionResult> Get()
		//{
		//    var operations = await this._operationService.GetAllAsync();
		//    return this.Ok(operations);
		//}
		[HttpGet("{id:int}")]
		public async Task<IActionResult> GetById(int id)
		{
			var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
			if (userIdClaim != null)
			{
				userId = int.Parse(userIdClaim.Value);
			}
			var user = await _userService.GetByIdAsync(userId);
			if (user == null)
			{
				return Unauthorized();
			}
			var operation = await this._operationService.GetByIdAsync(id, user.Id);
			if (operation == null)
			{
				return this.NotFound();
			}
			var model = this._mapper.Map<OperationModel>(operation);
			return this.Ok(model);
		}
		

		[HttpPost("create")]
		public async Task<IActionResult> Create([FromBody] OperationModel model, int cardId)
		{
			var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
			if (userIdClaim != null)
			{
				userId = int.Parse(userIdClaim.Value);
			}
			var user = await _userService.GetByIdAsync(userId);
			if (user == null)
			{
				return Unauthorized();
			}
			//if (!Regex.IsMatch(model.CreatedAt, @"^(?:4[0-9]{12}(?:[0-9]{3})?|5[1-5][0-9]{14})$"))
			//{
			//	throw new Exception("Invalid date");
			//}
			var operation = this._mapper.Map<Operation>(model);
			await _operationService.AddAsync(operation, user.Id, cardId);
			return this.Ok();
		}

		//[HttpPut("{id:int}/edit")]
		//public async Task<IActionResult> Update([FromBody] OperationModel model, [FromRoute] int id)
		//{
		//    var operation = this._mapper.Map<Operation>(model);
		//    var result = await this._operationService.TryUpdateAsync(id, operation);

		//    if (result)
		//    {
		//        return this.Ok();
		//    }
		//    else
		//    {
		//        return this.NotFound();
		//    }
		//}

		//[HttpDelete("{id:int}/delete")]
		//public async Task<IActionResult> DeleteAsync([FromRoute] int id)
		//{
		//    var operation = await this._operationService.GetByIdAsync(id);
		//    if (operation != null)
		//    {
		//        await this._operationService.DeleteAsync(operation);
		//        return this.Ok();
		//    }

		//    return this.NotFound();
		//}
		[HttpGet("enums")]
		public EnumOptions GetMyEnumOptions()
		{
			return MyEnumIncomes.GetMyExpensesEnum();
		}
	}
}
