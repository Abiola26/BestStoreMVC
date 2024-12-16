using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BestStoreMVC.Entity
{
    public class Book: BaseEntity
    {
        [MaxLength(100)]
        public string Title { get; set; } = "";

        [MaxLength(100)]
        public string Author { get; set; } = "";

        public Guid CategoryId { get; set; }

        public Category Category { get; set; }

        public int Pages { get; set; }

        public string Language { get; set; }

        public string ISBN { get; set; }

        [Precision(16, 2)]
        public decimal Price { get; set; }

        public string Description { get; set; } = "";

        [MaxLength(100)]
        public string ImageFileName { get; set; } = "";

        //public DateTime CreatedAt { get; set; }
    }
}
