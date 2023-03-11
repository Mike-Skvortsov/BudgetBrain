using Entities.Entities;
using System;

namespace proj.Models.DTO
{
	public class OperationDTOOneOperation
	{
		public string Name { get; set; }
		public decimal Sum { get; set; }
		public OperationType Type { get; set; }
		public DateTime CreatedAt { get; set; }
		public int Id { get; set; }
		public string CardName { get; set; }
		public CategoryModel Category { get; set; }

	}
}
