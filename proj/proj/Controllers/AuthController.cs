using Auth.Common;
using AutoMapper;
using BL.Services;
using DataAccess.Migrations;
using Entities.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using proj.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace proj.Controllers
{
	[Route("api")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IUserService _userService;
		private readonly IMapper _mapper;
		private readonly IConfiguration _configuration;
		public AuthController(IUserService userService, IMapper mapper, IConfiguration configuration)
		{
			this._mapper = mapper;
			this._userService = userService;
			this._configuration = configuration;
		}

		[HttpPost("registration")]
		public async Task<IActionResult> Create([FromBody] UserModel model)
		{
			var user = _mapper.Map<Entities.Entities.User>(model);
			var email = await _userService.GetUserByEmailAsync(user.Email);
			if (email == null)
			{
				await _userService.AddAsync(user);
				return Ok();
			}
			return this.Conflict("Email is already exists");
		}
		[HttpPost("logout")]
		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync();
			return Ok();
		}
		[Route("login")]
		[HttpPost]
		public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
		{
			var result = await this._userService.Search(request.Email, request.Password);
			if (result == null)
			{
				return NotFound("The user is not registered");
			}
			if (result != null)
			{
				var token = GenerateJWT(result);
				return Ok(new
				{
					accessToken = token,
				});
			}
			return BadRequest();
		}
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		[HttpGet("validate-token")]
		public IActionResult ValidateToken()
		{
			return Ok();
		}
		private string GenerateJWT(Entities.Entities.User user)
		{
			var claims = new List<Claim>()
			{
				new Claim(ClaimTypes.Email, user.Email),
				new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString())
			};
			var secretValue = _configuration.GetSection("Auth:Secret").Value;
			if (string.IsNullOrEmpty(secretValue))
			{
				throw new Exception("Auth:Secret is not set in the configuration");
			}
			var securityKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretValue));
			if (securityKey == null)
			{
				throw new Exception("Security key is null");
			}
			var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);


			var token = new JwtSecurityToken(
				claims: claims,
				expires: DateTime.Now.AddDays(1),
				signingCredentials: credentials);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}

	}
}
