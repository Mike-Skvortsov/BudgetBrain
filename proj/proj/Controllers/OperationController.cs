using AutoMapper;
using BL.Services;
using Entities.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using proj.Models;
using System.Threading.Tasks;

namespace proj.Controllers
{
    
    [ApiController]
    [Route("operation")]
	[Authorize]
	public class OperationController : ControllerBase
    {
        private readonly IOperationService _operationService;
        private readonly ICardService _cardService;
        private readonly IMapper _mapper;

        public OperationController(IOperationService operationService, ICardService cardService, IMapper mapper)
        {
            this._operationService = operationService;
            this._cardService = cardService;
            this._mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var operations = await this._operationService.GetAllAsync();
            return this.Ok(operations);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var operation = await this._operationService.GetByIdAsync(id);
            if (operation == null)
            {
                return this.NotFound();
            }
            var model = this._mapper.Map<OperationModel>(operation);
            return this.Ok(model);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] OperationModel model)
        {
            var operation = this._mapper.Map<Operation>(model);
            
            await _operationService.AddAsync(operation);
            return this.Ok();
        }

        [HttpPut("{id:int}/edit")]
        public async Task<IActionResult> Update([FromBody] OperationModel model, [FromRoute] int id)
        {
            var operation = this._mapper.Map<Operation>(model);
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
            var operation = await this._operationService.GetByIdAsync(id);
            if (operation != null)
            {
                await this._operationService.DeleteAsync(operation);
                return this.Ok();
            }

            return this.NotFound();
        }
    }
}
