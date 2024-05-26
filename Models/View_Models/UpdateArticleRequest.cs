using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Novitas_Blog.Models.View_Models
{
    public class UpdateArticleRequest
    {
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public string Blog_Url { get; set; }
        [Required]
        public string? Featured_Image_Url { get; set; }
        [Required]
        public string Author { get; set; }
        public DateTime Published_Date { get; set; }

        public bool Is_Visible { get; set; }
        public bool Is_Featured { get; set; } = false;
        public IEnumerable<SelectListItem> Category { get; set; }
        [Required]
        public string Selected_Category { get; set; }

        //For Display Tags
        public IEnumerable<SelectListItem> Tags { get; set; }
        //Capture Tags
        public string[]? Selected_Tags { get; set; } = Array.Empty<string>();
    }
}
