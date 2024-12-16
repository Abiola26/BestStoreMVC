using System.ComponentModel.DataAnnotations;

namespace BestStoreMVC.Models.RequestModels
{
    public class CategoryRequestModel
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Category name is required")]
        [StringLength(100, ErrorMessage = "Category name cannot be longer than 100 characters")]
        public string Name { get; set; }
    }
}
