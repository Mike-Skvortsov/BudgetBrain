using Auth.Common;
using BL.Services;
using Entities.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using proj.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace proj.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IUserService _userService;
		private readonly IOptions<AuthOptions> authOptions;

		public AuthController(IUserService userService, IOptions<AuthOptions> authOptions)
		{
			this._userService = userService;
			this.authOptions = authOptions;
		}
		[Route("login")]
		[HttpPost]
		public async Task<IActionResult> Login([FromBody] Login request)
		{
			var result = await this._userService.Search(request.Email, request.Password);
			if (result != null)
			{
				var token = GenerateJWT(result);

				return Ok(new
				{
					accessToken = token
				});
			}
			return Unauthorized();
		}

		private string GenerateJWT(User user)
		{
			var authParams = authOptions.Value;

			var securityKey = authParams.GetSynnetricSecurityKey();
			var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

			var claims = new List<Claim>()
			{
				new Claim(JwtRegisteredClaimNames.Email, user.Email),
				new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString())
			};

			var token = new JwtSecurityToken(authParams.Issuer,
				authParams.Audience,
				claims,
				expires: DateTime.Now.AddSeconds(authParams.TokenLifetime),
				signingCredentials: credentials);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}
