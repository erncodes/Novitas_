using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Novitas_Blog.Data;

namespace Novitas_Blog.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly NovitasAuthDBContext _novitasAuthDBContext;

        public UserRepository(NovitasAuthDBContext novitasAuthDBContext)
        {
            this._novitasAuthDBContext = novitasAuthDBContext;
        }

        public async Task<IEnumerable<IdentityUser>> GetAllUsers()
        {
            var users = await _novitasAuthDBContext.Users.ToListAsync();
            var superAdmin = await _novitasAuthDBContext.Users.FirstOrDefaultAsync(x => x.Email == "superadmin@novitas.com");
            if(superAdmin != null)
            {
                users.Remove(superAdmin);
                return users;
            }
            return users;
        }
    }
}
