using Entities.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace proj.Models
{
    public class CardModel
    {
        public string NumberCard { get; set; }
        public decimal CardAmount { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
    }
}
