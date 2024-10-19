using Microsoft.EntityFrameworkCore;
using Novitas_Blog.Data;
using Novitas_Blog.Models.Domain_Models;

namespace Novitas_Blog.Repositories
{
    public class BlogArticleRepository : IBlogArticleRepository
    {
        private readonly NovitasDBContext _novitasDBContext;
        public BlogArticleRepository(NovitasDBContext _novitasDBContext)
        {
            this._novitasDBContext = _novitasDBContext;
        }
        //Implement IBlogArticleRepository
        public async Task<BlogArticle> AddAsync(BlogArticle article)
        {
            await _novitasDBContext.Articles.AddAsync(article);
            await _novitasDBContext.SaveChangesAsync();
            return article;
        }
        public async Task<BlogArticle?> UpdateAsync(BlogArticle article)
        {
            var selectedArticle = await _novitasDBContext.Articles.Include(x => x.Blog_Tags).FirstOrDefaultAsync(x => x.Id == article.Id);
            if (selectedArticle != null)
            {
                selectedArticle.Id = article.Id;
                selectedArticle.Title = article.Title;
                selectedArticle.Author = article.Author;
                selectedArticle.Content = article.Content;
                selectedArticle.Blog_Url = article.Blog_Url;
                selectedArticle.Description = article.Description;
                selectedArticle.Featured_Image_Url = article.Featured_Image_Url;
                selectedArticle.Published_Date = article.Published_Date;
                selectedArticle.Is_Featured = article.Is_Featured;
                selectedArticle.Is_Visible = article.Is_Visible;
                selectedArticle.Blog_Tags = article.Blog_Tags;
                selectedArticle.Category = article.Category;

                await _novitasDBContext.SaveChangesAsync();

                return selectedArticle;
            }
            return null;
        }
        public async Task<BlogArticle?> DeleteAsync(Guid Id)
        {
            var article = await _novitasDBContext.Articles.FirstOrDefaultAsync(x => x.Id == Id);

            if (article != null)
            {
                _novitasDBContext.Articles.Remove(article);
                await _novitasDBContext.SaveChangesAsync();
                return article;
            }
            return null;
        }
        public async Task<BlogArticle?> GetBlogByIdAsync(Guid Id)
        {
            var article = await _novitasDBContext.Articles.Include(x => x.Blog_Tags).FirstOrDefaultAsync(x => x.Id == Id);

            return article != null ? article : null;
        }
        public async Task<BlogArticle?> GetBlogByHandleAsync(string urlHandle)
        {
            return await _novitasDBContext.Articles.Include(x => x.Blog_Tags).Include(x => x.Comments).FirstOrDefaultAsync(x => x.Blog_Url == urlHandle);
        }
        public async Task<IEnumerable<BlogArticle>> GetAllAsync(string? searchQuery)
        {
            var articles = _novitasDBContext.Articles.Include(x => x.Category).Include(x => x.Blog_Tags).AsQueryable();
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                articles = articles.Where(x => x.Title.Contains(searchQuery)
                || x.Category.CategoryName.Contains(searchQuery) || x.Blog_Tags.Any(x => x.Name.Contains(searchQuery)));
            }
            return await articles.ToListAsync();
        }
        public async Task<IEnumerable<BlogArticle>> GetAllByCategoryAsync(string? searchQuery)
        {
            var articles = _novitasDBContext.Articles.Include(x => x.Category).Include(x => x.Blog_Tags).AsQueryable();
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                articles = articles.Where(x => x.Category.CategoryName.Contains(searchQuery) || x.Title.Contains(searchQuery));
            }
            return await articles.ToListAsync();
        }
    }
}

