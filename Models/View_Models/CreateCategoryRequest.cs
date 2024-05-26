using System.ComponentModel.DataAnnotations;

namespace Novitas_Blog.Models.View_Models
{
    public class CreateCategoryRequest
    {
        [Required]
        public string CategoryName { get; set; }

    }
}
