using Entities.Entities;
using System;

namespace proj.Models.DTO
{
	public class OperationDTO1
	{
		public int CardId { get; set; }
		public string Name { get; set; }
		public string CategoryName { get; set; }
		public decimal Sum { get; set; }
		public OperationType Type { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}
