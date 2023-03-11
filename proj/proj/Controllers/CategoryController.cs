using AutoMapper;
using BL.Services;
using Entities.Entities;
using Microsoft.AspNetCore.Mvc;
using proj.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace proj.Controllers
{
	[Route("api/category")]
	[ApiController]
	public class CategoryController : ControllerBase
	{

		private readonly ICategoryService _categoryService;
		private readonly IMapper _mapper;

		public CategoryController(ICategoryService categoryService, IMapper mapper)
		{
			_categoryService = categoryService;
			_mapper = mapper;
		}

		[HttpGet]
		[Route("{typeOperation:int}")]
		public async Task<IActionResult> GetCategory(OperationType typeOperation)
		{
			var category = await _categoryService.GetByCategory(typeOperation);
			if(category == null)
			{
				return NotFound("this category doesn`t exist!");
			}
			var model = this._mapper.Map<ICollection<CategoryModel>>(category);
			return Ok(model);
		}
	}
}
