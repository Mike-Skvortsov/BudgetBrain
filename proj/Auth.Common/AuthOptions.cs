using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace Auth.Common
{
	public class AuthOptions
	{
		public string Secret { get; set; }
		public string Audience { get; set; }
		public string Issuer { get; set; }
		public int TokenLifetime { get; set; }
		public SymmetricSecurityKey GetSynnetricSecurityKey()
		{
			if (string.IsNullOrEmpty(Secret))
			{
					throw new ArgumentNullException(nameof(Secret), "Secret is null or empty");
			}
			return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Secret));
		}
	}
}
