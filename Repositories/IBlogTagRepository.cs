using Novitas_Blog.Models.Domain_Models;

namespace Novitas_Blog.Repositories
{
    public interface IBlogTagRepository
    {
        public Task<IEnumerable<BlogTag>> GetAllAsync(string? seachQuery = null);
        public Task<BlogTag> GetTagByIdAsync(Guid Id);

        public Task<BlogTag> AddAsync(BlogTag blogArticle);
        public Task<BlogTag> UpdateAsync(BlogTag blogArticle);
        public Task<BlogTag> DeleteAsync(Guid Id);
    }
}
