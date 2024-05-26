using Novitas_Blog.Models.Domain_Models;

namespace Novitas_Blog.Repositories
{
    public interface ICommentRepository
    {
        public Task<Comment> AddAsync(Comment comment);
        public Task<IEnumerable<Comment>> GetCommentsAsync(Guid Id);
    }
}
