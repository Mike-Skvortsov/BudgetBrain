using AutoMapper;
using BL.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using proj.Models;
using Entities.Entities;
using proj.Models.DTO;
using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;

namespace proj.Controllers
{
	[Route("api/card")]
	[ApiController]
	[Authorize]
	public class CardController : ControllerBase
	{
		private readonly ICardService _cardService;
		private readonly IUserService _userService;
		private readonly IMapper _mapper;
		private readonly IColorService _colorService;
		private int userId;
		public CardController(ICardService cardService, IUserService _userService, IMapper mapper, IColorService colorService)
		{
			this._cardService = cardService;
			this._userService = _userService;
			this._mapper = mapper;
			this._colorService = colorService;
		}
		[HttpGet]
		[Route("")]
		public async Task<IActionResult> GetAllCards()
		{
			var user = await GetUserAsync();
			if (user == null)
			{
				return Unauthorized();
			}
			var cards = await _cardService.GetByUserIdAsync(user.Id);

			var model = _mapper.Map<ICollection<CardDTO1>>(cards);
			return Ok(model);
		}
		[HttpGet("{id:int}")]
		public async Task<IActionResult> GetById(int id)
		{
			var user = await GetUserAsync();
			if (user == null)
			{
				return Unauthorized();
			}
			var card = await _cardService.GetByIdAsync(id, user.Id);
			if (card == null)
			{
				return this.NotFound("Card not found");
			}
			
			var model = _mapper.Map<CardDTOColorOperation>(card);
			
			return Ok(model);
		}
		[HttpPost("create")]
		public async Task<IActionResult> Create([FromBody] CardModel model)
		{
			var user = await GetUserAsync();
			if (user == null)
			{
				return Unauthorized();
			}
			var card = this._mapper.Map<Card>(model);
			card.ColorCard = await _colorService.GetByColorValueAsync(model.ColorValue);
			card.User = user;
			if (card == null)
			{
				return this.NotFound("Card not found");
			}
			var cardNumber = await _cardService.GetByCardNumber(card.NumberCard, user.Id);
			if(cardNumber != null)
			{
				return this.Conflict("This card number already exists");
			}
			if (card.CardAmount < 0)
			{
				throw new Exception("You cannot enter a card balance less than zero!");
			}
			
			await _cardService.AddAsync(card);
			return Ok();
		}
		[HttpPut("{id:int}/edit")]
		public async Task<IActionResult> Update([FromBody] CardModel model, [FromRoute] int id)
		{
			try
			{
				var user = await GetUserAsync();
				if (user == null)
				{
					return Unauthorized();
				}
				var card = this._mapper.Map<Card>(model);
				if (card.CardAmount < 0)
				{
					throw new Exception("The balance on the card cannot be negative");
				}
				if (!Regex.IsMatch(model.NumberCard, @"^(?:4[0-9]{12}(?:[0-9]{3})?|5[1-5][0-9]{14})$"))
				{
					throw new Exception("Invalid card number");
				}
				var cardNumber = await _cardService.GetByCardNumber(card.NumberCard, user.Id);
				if (cardNumber != null)
				{
					return this.Conflict("This card number already exists");
				}
				card.ColorCard = await _colorService.GetByColorValueAsync(model.ColorValue);
				var result = await this._cardService.TryUpdateAsync(id, card, user.Id);
				if (result)
				{
					return this.Ok();
				}
				else
				{
					return this.NotFound();
				}
			}
			catch (Exception ex)
			{
				return this.BadRequest(ex.Message);
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
			var card = await this._cardService.GetByIdAsync(id, user.Id);
			if (card == null)
			{
				return this.NotFound("Card not found");
			}
			await this._cardService.DeleteAsync(card);
			return this.Ok();
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
	}
}
