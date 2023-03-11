using AutoMapper;
using BL.Services;
using Microsoft.AspNetCore.Mvc;
using proj.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace proj.Controllers
{
	[Route("api/colors")]
	[ApiController]
	public class ColorController : ControllerBase
	{
		private readonly IColorService _colorService;
		private readonly IMapper _mapper;
		public ColorController(IColorService colorService, IMapper mapper)
		{
			this._colorService = colorService;
			this._mapper = mapper;
		}
		[HttpGet("")]
		public async Task<IActionResult> GetColors()
		{
			var colors = await _colorService.GetAllColorAsync();
			var model = this._mapper.Map<ICollection<ColorModel>>(colors);
			return Ok(colors);
		}
	}
}
