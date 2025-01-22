using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExampleProject4.Core.Entities
{
    [Table("OrderItems")]
    public class OrderItem
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }
        [Column("OrderId")]
        public int OrderId { get; set; }
        [Column("ProductName")]
        public string ProductName { get; set; }
        [Column("Quantity")]
        public int Quantity { get; set; }

        [ForeignKey("OrderId")]
        public Order Order { get; set; }
    }
}
