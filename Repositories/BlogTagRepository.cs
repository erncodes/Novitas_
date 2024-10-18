using Microsoft.EntityFrameworkCore;
using Novitas_Blog.Data;
using Novitas_Blog.Models.Domain_Models;

namespace Novitas_Blog.Repositories
{
    public class BlogTagRepository : IBlogTagRepository
    {
        private readonly NovitasDBContext _novitasDBContext;
        public BlogTagRepository(NovitasDBContext novitasDBContext)
        {
            _novitasDBContext = novitasDBContext;
        }
        public async Task<BlogTag> AddAsync(BlogTag tag)
        {
            await _novitasDBContext.BlogTags.AddAsync(tag);
            await _novitasDBContext.SaveChangesAsync();
            return tag;
        }
        public async Task<BlogTag> UpdateAsync(BlogTag blogTag)
        {
            var selectedTag = await _novitasDBContext.BlogTags.FindAsync(blogTag.Id);
            if (selectedTag != null)
            {
                selectedTag.Name = blogTag.Name;
                selectedTag.Articles = blogTag.Articles;
                await _novitasDBContext.SaveChangesAsync();
                return selectedTag;
            }
            return null;
        }
        public async Task<BlogTag> DeleteAsync(Guid Id)
        {
            var tag = await _novitasDBContext.BlogTags.FirstOrDefaultAsync(x => x.Id == Id);

            if (tag != null)
            {
                _novitasDBContext.BlogTags.Remove(tag);
                await _novitasDBContext.SaveChangesAsync();
                return tag;
            }
            return null;
        }
        public async Task<BlogTag> GetTagByIdAsync(Guid Id)
        {
            var tag = await _novitasDBContext.BlogTags.FindAsync(Id);

            return tag != null ? tag : null;
        }
        public async Task<IEnumerable<BlogTag>> GetAllAsync(string? searchQuery)
        {
            var query = _novitasDBContext.BlogTags.AsQueryable();
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                query = query.Where(x => x.Name.Contains(searchQuery));
            }
            return await query.ToListAsync();
        }
    }
}
