using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Entities
{
    public class Operation : BaseEntity
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal Sum { get; set; }
        public OperationType Type { get; set; }
        [ForeignKey("Card")]
        public int CardId { get; set; }
        public virtual Card Card { get; set; } 
    }
}
