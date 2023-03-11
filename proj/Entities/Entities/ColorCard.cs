using System.Collections.Generic;

namespace Entities.Entities
{
	public class ColorCard
	{
		public int Id { get; set; }
		public string Value { get; set; }
		public string Label { get; set; }
		public virtual List<Card> Cards { get; private set; }

	}
}
