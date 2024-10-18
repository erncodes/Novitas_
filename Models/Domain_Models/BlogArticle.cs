namespace Novitas_Blog.Models.Domain_Models
{
    public class BlogArticle
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string Blog_Url { get; set; }
        public string? Featured_Image_Url { get; set; }
        public string Author { get; set; }
        public DateTime Published_Date { get; set; } = DateTime.Now;
        public string? TimeString { get; set; }
        public bool Is_Visible { get; set; }
        public bool Is_Featured { get; set; } = false;
        public Category Category { get; set; }

        public ICollection<BlogTag?> Blog_Tags { get; set; }
        public ICollection<Comment?> Comments { get; set; }
    }
}
