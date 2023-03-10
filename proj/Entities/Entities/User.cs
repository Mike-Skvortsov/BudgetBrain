using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace Entities.Entities
{
	public class User : BaseEntity
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public string Photo { get; set; }
		public virtual ICollection<Card> Cards { get; set; }
	}
}
