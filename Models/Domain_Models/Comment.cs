namespace Novitas_Blog.Models.Domain_Models
{
    public class Comment
    {
        public Guid Id { get; set; }
        public Guid PostId { get; set; }
        public Guid UserId { get; set; }
        public string Descripton { get; set; }
        public DateTime DateAdded { get; set; }
    }
}
