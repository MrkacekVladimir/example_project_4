using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleProject4.Core.Entities
{
   [Table("Orders")]
    public class Order
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("CreatedOn")]
        public DateTime CreatedOn { get; set; }

        [Column("PaidOn")]
        public DateTime? PaidOn { get; set; }

        public List<OrderItem> OrderItems { get; set; }
    }
}
