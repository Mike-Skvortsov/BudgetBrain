using AutoMapper;
using BL.Services;
using Entities.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using proj.Models;
using System.Linq;
using System.Threading.Tasks;

namespace proj.Controllers
{
    [ApiController]
    [Route("card")]
    [Authorize]
    public class CardController : ControllerBase
    {
        private readonly ICardService _cardService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public CardController(ICardService cardService, IUserService _userService, IMapper mapper)
        {
            this._cardService = cardService;
            this._userService = _userService;
            this._mapper = mapper;
        }
        [HttpGet]
		public async Task<IActionResult> Get()
		{
			var cards = await this._cardService.GetAllAsync();
			var model = cards.Select(x => this._mapper.Map<CardModel>(x)).ToList();
			return this.Ok(cards);
		}
		[HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var card = await this._cardService.GetByIdAsync(id);
            if (card == null)
            {
                return this.NotFound();
            }
            var model = this._mapper.Map<CardModel>(card);
            return this.Ok(model);
        }

        [HttpPost("create")]    
        public async Task<IActionResult> Create([FromBody] CardModel model)
        {
            User user = await _userService.GetByIdAsync(model.UserId);
            if (user == null)
            {
                return this.BadRequest();
            }
            var card = this._mapper.Map<Card>(model);
            card.User = user;
            if(card.CardAmount < 0)
            {
                return BadRequest();
            }
            else
            {
                await _cardService.AddAsync(card);
                return this.Ok();
            }
        }

        [HttpPut("{id:int}/edit")]
        public async Task<IActionResult> Update([FromBody] CardModel model, [FromRoute] int id)
        {
            var card = this._mapper.Map<Card>(model);
            var result = await this._cardService.TryUpdateAsync(id, card);

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
            var card = await this._cardService.GetByIdAsync(id);
            if (card != null)
            {
                await this._cardService.DeleteAsync(card);

                return this.Ok();
            }

            return this.NotFound();
        }
    }
}
