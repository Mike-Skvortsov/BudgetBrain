using AutoMapper;
using BL.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using proj.Models;
using Entities.Entities;
using System.Linq;
using Newtonsoft.Json;
using proj.Models.DTO;
using System.Text.RegularExpressions;
using System;

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
		private int userId;

		public CardController(ICardService cardService, IUserService _userService, IMapper mapper)
		{
			this._cardService = cardService;
			this._userService = _userService;
			this._mapper = mapper;
		}

		[HttpGet]
		[Route("")]
		public async Task<IActionResult> GetAllCards()
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
			var cards = await _cardService.GetByUserIdAsync(user.Id);

			var filteredCards = cards.Select(c => new CardDTO1 { NumberCard = c.NumberCard, CardAmount = c.CardAmount, CardName = c.CardName, Id = c.Id });
			return Ok(filteredCards);
		}
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
			var card = await _cardService.GetByIdAsync(id, user.Id);
			if (card == null)
			{
				return this.NotFound("Card not found");
			}
			var cardDTO1 = new { card.NumberCard, card.CardAmount, card.Id, card.CardName, card.Operations };
			return Ok(cardDTO1);
		}

		[HttpPost("create")]
		public async Task<IActionResult> Create([FromBody] CardModel model)
		{
			var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
			if (userIdClaim == null)
			{
				return Unauthorized();
			}
			if (userIdClaim != null)
			{
				userId = int.Parse(userIdClaim.Value);
			}
			var user = await _userService.GetByIdAsync(userId);
			if (user == null)
			{
				return this.NotFound("User not found");
			}
			//if (!Regex.IsMatch(model.CardNumber, @"^(?:4[0-9]{12}(?:[0-9]{3})?|5[1-5][0-9]{14})$"))
			//{
			//	throw new Exception("Invalid card number");
			//}
			var card = this._mapper.Map<Card>(model);
			card.User = user;
			card.User.Id = user.Id;
			if (card == null)
			{
				return this.NotFound("Card not found");
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
				var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
				if (userIdClaim != null)
				{
					userId = int.Parse(userIdClaim.Value);
				}
				var user = await _userService.GetByIdAsync(userId);
				if (user == null)
				{
					return this.NotFound("User not found");
				}
				var card = this._mapper.Map<Card>(model);
				card.User = user;
				if (card.CardAmount < 0)
				{
					throw new Exception("The balance on the card cannot be negative");
				}
				if (!Regex.IsMatch(model.NumberCard, @"^(?:4[0-9]{12}(?:[0-9]{3})?|5[1-5][0-9]{14})$"))
				{
					throw new Exception("Invalid card number");
				}
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
			var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
			if (userIdClaim != null)
			{
				userId = int.Parse(userIdClaim.Value);
			}
			var user = await _userService.GetByIdAsync(userId);
			if (user == null)
			{
				return this.NotFound("User not found");
			}
			var card = await this._cardService.GetByIdAsync(id, user.Id);
			if (card == null)
			{
				return this.NotFound("Card not found");
			}
			await this._cardService.DeleteAsync(card);
			return this.Ok();
		}
	}
}
