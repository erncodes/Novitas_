using Novitas_Blog.Models.Domain_Models;

namespace Novitas_Blog.Models.View_Models
{
    public class HomeViewModel
    {
        public IEnumerable<BlogArticle> General_Articles { get; set; }
        public IEnumerable<BlogArticle> Latest_General { get; set; }

        public IEnumerable<BlogArticle> Featured_Articles { get; set; }
        public BlogArticle Latest_Featured { get; set; }
    }
}
