using System.Collections.Generic;

namespace Entities.Entities
{
	public class Category
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Image { get; set; }
		public OperationType TypeCategory { get; set; }
		public virtual List<Operation> Operations { get; private set; }

	}
}
