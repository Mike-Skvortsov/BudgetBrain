using Entities.Entities;
using System.Drawing;

namespace proj.Models.DTO
{
	public class CardDTO1
	{
		public int Id { get; set; }
		public string CardName { get; set; }
		public decimal CardAmount { get; set; }
		public string NumberCard { get; set; }
		public ColorModel Color { get; set; }
	}
}
