using Novitas_Blog.Models.Domain_Models;
using System.ComponentModel.DataAnnotations;

namespace Novitas_Blog.Models.View_Models
{
    public class UpdateTagRequest
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public ICollection<BlogArticle?> Articles { get; set; }

    }
}
