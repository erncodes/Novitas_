using System.ComponentModel.DataAnnotations;

namespace Novitas_Blog.Models.View_Models
{
    public class CreateCommentRequest
    {
        public Guid PostId { get; set; }
        public Guid UserId { get; set; }
        [Required]
        public string Descripton { get; set; }
        public string urlHandle { get; set; }
        public DateTime DateAdded { get; set; }

    }
}
