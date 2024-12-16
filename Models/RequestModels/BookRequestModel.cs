using System.ComponentModel.DataAnnotations;

namespace BestStoreMVC.Models.RequestModels
{
    public class BookRequestModel
    {
        public Guid Id { get; set; }
        [Required, MaxLength(100)]
        public string Title { get; set; } = "";

        [Required, MaxLength(100)]
        public string Author { get; set; } = "";

        public Guid CategoryId { get; set; }

        [Required]
        public int Pages { get; set; }

        [Required]
        public string ISBN { get; set; }

        [Required]
        public string Language { get; set; }

        [Required, Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        public string Description { get; set; } = "";

        public IFormFile? ImageFile { get; set; }
    }
}
