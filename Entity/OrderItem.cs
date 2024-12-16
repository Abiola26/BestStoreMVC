using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace BestStoreMVC.Entity
{
    [Table("OrderItems")]
    public class OrderItem : BaseEntity
    {
        public int Quantity { get; set; }

        [Precision(16, 2)]
        public decimal UnitPrice { get; set; }

        // navigation property
        public Book Book { get; set; } = new Book();
    }
}
