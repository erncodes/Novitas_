using Microsoft.AspNetCore.Identity;

namespace Novitas_Blog.Repositories
{
    public interface IUserRepository
    {
        public Task<IEnumerable<IdentityUser>> GetAllUsers();
    }
}
