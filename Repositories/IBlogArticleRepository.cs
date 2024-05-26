using Novitas_Blog.Models.Domain_Models;

namespace Novitas_Blog.Repositories
{
    public interface IBlogArticleRepository
    {
        public Task<IEnumerable<BlogArticle>> GetAllAsync(string? searchQuery);
        public Task<IEnumerable<BlogArticle>> GetAllByCategoryAsync(string? searchQuery);
        public Task<BlogArticle?> GetBlogByIdAsync(Guid Id);
        public Task<BlogArticle?> GetBlogByHandleAsync(String urlHandle);

        public Task<BlogArticle> AddAsync(BlogArticle blogArticle);
        public Task<BlogArticle?> UpdateAsync(BlogArticle blogArticle);
        public Task<BlogArticle?> DeleteAsync(Guid Id);
    }
}
