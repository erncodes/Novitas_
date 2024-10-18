using Novitas_Blog.Models.Domain_Models;

namespace Novitas_Blog.Repositories
{
    public interface ICategoryRepository
    {
        public Task<IEnumerable<Category>> GetAllAsync(string? searchQuery);
        public Task<Category> GetCategoryByIdAsync(Guid Id);
        public Task<Category> AddAsync(Category category);
        public Task<Category> UpdateAsync(Category category);
        public Task<Category> DeleteAsync(Guid Id);

    }
}
