namespace Novitas_Blog.Models.Domain_Models
{
    public class BlogTag
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<BlogArticle?> Articles { get; set; }
    }
}
