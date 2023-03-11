using System.Collections.Generic;

namespace proj.Models.DTO
{
	public class CardDTOColorOperation
	{
		public int Id { get; set; }
		public string CardName { get; set; }
		public decimal CardAmount { get; set; }
		public string NumberCard { get; set; }
		public ColorModel Color { get; set; }
		public ICollection<OperationModel> Operations { get; set; }
	}
}
