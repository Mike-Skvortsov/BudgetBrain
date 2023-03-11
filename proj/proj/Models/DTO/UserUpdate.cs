using Microsoft.AspNetCore.Http;

namespace proj.Models.DTO
{
	public class UserUpdate
	{
		public string Email { get; set; }
		public string Password { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Photo { get; set; }
	}
}
