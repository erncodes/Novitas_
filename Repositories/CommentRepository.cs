using Microsoft.EntityFrameworkCore;
using Novitas_Blog.Data;
using Novitas_Blog.Models.Domain_Models;

namespace Novitas_Blog.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly NovitasDBContext _novitasDBContext;

        public CommentRepository(NovitasDBContext novitasDBContext)
        {
            this._novitasDBContext = novitasDBContext;
        }
        public async Task<Comment> AddAsync(Comment comment)
        {
            await _novitasDBContext.Comments.AddAsync(comment);
            await _novitasDBContext.SaveChangesAsync();

            return comment;

        }

        public async Task<IEnumerable<Comment>> GetCommentsAsync(Guid Id)
        {
            return await _novitasDBContext.Comments.Where(x => x.PostId ==  Id).ToListAsync();
        }
    }
}
