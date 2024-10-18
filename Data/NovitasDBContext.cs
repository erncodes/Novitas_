using Microsoft.EntityFrameworkCore;
using Novitas_Blog.Models.Domain_Models;

namespace Novitas_Blog.Data
{
    public class NovitasDBContext : DbContext
    {
        public NovitasDBContext(DbContextOptions<NovitasDBContext> options) : base(options)
        { }
        public DbSet<BlogArticle> Articles { get; set; }
        public DbSet<BlogTag> BlogTags { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}
