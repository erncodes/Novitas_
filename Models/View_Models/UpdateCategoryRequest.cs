using System.ComponentModel.DataAnnotations;

namespace Novitas_Blog.Models.View_Models
{
    public class UpdateCategoryRequest
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string CategoryName { get; set; }
    }
}
