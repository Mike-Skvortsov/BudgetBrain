using Entities.Entities;
using System;

namespace proj.Models
{
    public class OperationModel
    {
        public string Name { get; set; }
		public decimal Sum { get; set; }
        public OperationType Type { get; set; }
        public DateTime CreatedAt { get; set; } 
        public int Id { get; set; }
		public CategoryModel Category { get; set; }

	}
}
