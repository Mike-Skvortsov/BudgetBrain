using Entities.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace proj.Models
{
    public class OperationModel
    {
        public string Name { get; set; }
        public decimal Sum { get; set; }
        public OperationType Type { get; set; }
        [ForeignKey("Card")]
        public int CardId { get; set; }
    }
}
