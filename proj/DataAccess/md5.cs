using System;
using System.Text;
using System.Security.Cryptography;

namespace DataAccess
{
	class md5
	{
		public static string HashPassword(string password)
		{
			MD5 md5 = MD5.Create();
			byte[] b = Encoding.ASCII.GetBytes(password);
			byte[] hash = md5.ComputeHash(b);
			StringBuilder stringBuilder = new StringBuilder();
			foreach (var a in hash)
			{
				stringBuilder.Append(a.ToString("X2"));
			}

			return Convert.ToString(stringBuilder);
		}
	}
}
