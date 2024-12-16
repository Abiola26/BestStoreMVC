using System.ComponentModel.DataAnnotations;

namespace BestStoreMVC.Dto
{
    public class BookDto
    {
        public Guid Id { get; set; }
        [Required, MaxLength(100)]
        public string Title { get; set; } = "";

        [Required, MaxLength(100)]
        public string Author { get; set; } = "";

        [Required, MaxLength(100)]
        public string CategoryName { get; set; } = "";
        public Guid CategoryId { get; set; }
        [Required]
        public int Pages { get; set; }
        [Required]  
        public string ISBN { get; set; }
        [Required]
        public string Language { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public string Description { get; set; } = "";
        public string ImageFileName { get; set; } = "";

        public IFormFile? ImageFile { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
