using Novitas_Blog.Models.Domain_Models;

namespace Novitas_Blog.Models.View_Models
{
    public class ArticleDetailsViewModel
    {
        public BlogArticle MainArticle { get; set; }
        public string Comment { get; set; }
        public string UrlHandle { get; set; }
        public IEnumerable<BlogArticle> RelatedArticles { get; set; }
        public IEnumerable<CommentView?> Comments { get; set; }
    }
}
