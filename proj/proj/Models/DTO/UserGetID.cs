using Microsoft.AspNetCore.Http;

namespace proj.Models.DTO
{
	public class UserGetID
	{
		public string Id { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
	}
}
