using Microsoft.AspNetCore.Mvc.Rendering;
using Novitas_Blog.Models.Domain_Models;
using System.ComponentModel.DataAnnotations;

namespace Novitas_Blog.Models.View_Models
{
    public class CreateTagRequest
    {
        [Required]
        public string Name { get; set; }
    }
}
