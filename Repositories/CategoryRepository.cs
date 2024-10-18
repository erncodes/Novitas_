using Microsoft.EntityFrameworkCore;
using Novitas_Blog.Data;
using Novitas_Blog.Models.Domain_Models;

namespace Novitas_Blog.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly NovitasDBContext _novitasDBContext;
        public CategoryRepository(NovitasDBContext novitasDBContext)
        {
            _novitasDBContext = novitasDBContext;
        }
        public async Task<Category> AddAsync(Category category)
        {
            await _novitasDBContext.Categories.AddAsync(category);
            await _novitasDBContext.SaveChangesAsync();
            return category;
        }
        public async Task<Category> DeleteAsync(Guid Id)
        {
            var category = await _novitasDBContext.Categories.FirstOrDefaultAsync(x => x.Id == Id);

            if (category != null)
            {
                _novitasDBContext.Categories.Remove(category);
                await _novitasDBContext.SaveChangesAsync();
                return category;
            }
            return null;
        }
        public async Task<Category> GetCategoryByIdAsync(Guid Id)
        {
            var category = await _novitasDBContext.Categories.FindAsync(Id);

            return category != null ? category : null;
        }
        public async Task<IEnumerable<Category>> GetAllAsync(string? searchQuery)
        {
            var query = _novitasDBContext.Categories.AsQueryable();
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                query = query.Where(x => x.CategoryName.Contains(searchQuery));
            }
            return await query.ToListAsync();
        }
        public async Task<Category> UpdateAsync(Category category)
        {
            var selectedCategory = await _novitasDBContext.Categories.FindAsync(category.Id);

            if (selectedCategory != null)
            {
                selectedCategory.CategoryName = category.CategoryName;

                await _novitasDBContext.SaveChangesAsync();
                return selectedCategory;
            }
            return null;
        }
    }
}
