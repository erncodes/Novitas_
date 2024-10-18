using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Novitas_Blog.Data
{
    public class NovitasAuthDBContext : IdentityDbContext
    {
        public NovitasAuthDBContext(DbContextOptions<NovitasAuthDBContext> options) : base(options)
        { }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            var userRoleId = "fcbc040e-3725-4a24-9dcc-d7f61532e4cb";
            var adminRoleId = "91e89623-04ac-45ed-a9ab-691729aa7695";
            var superAdminRoleId = "989c2b0c-f6a9-4cfa-8ef9-72484c5aa18e";

            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Name = "SuperAdmin",
                    NormalizedName = "SuperAdmin",
                    Id = superAdminRoleId,
                    ConcurrencyStamp = superAdminRoleId
                },
                new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "Admin",
                    Id = adminRoleId,
                    ConcurrencyStamp = adminRoleId
                },
                new IdentityRole
                {
                    Name = "User",
                    NormalizedName = "User",
                    Id = userRoleId,
                    ConcurrencyStamp = userRoleId
                },
            };

            builder.Entity<IdentityRole>().HasData(roles);

            var superAdminId = "9500ca94-8028-4a1a-8b0d-4f069bc22cbf";
            var superAdminUser = new IdentityUser
            {
                UserName = "SuperAdmin",
                Email = "superadmin@novitas.com",
                NormalizedEmail = "superadmin@novitas.com".ToUpper(),
                NormalizedUserName = "SuperAdmin",
                Id = superAdminId
            };
            superAdminUser.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(superAdminUser, "Super@12345");

            builder.Entity<IdentityUser>().HasData(superAdminUser);

            var superAdminUserRoles = new List<IdentityUserRole<string>>
            {
                new IdentityUserRole<string>
                {
                    RoleId = superAdminRoleId,
                    UserId = superAdminId
                },
                new IdentityUserRole<string>
                {
                    RoleId = userRoleId,
                    UserId = superAdminId
                },
                new IdentityUserRole<string>
                {
                    RoleId = adminRoleId,
                    UserId = superAdminId
                },
            };

            builder.Entity<IdentityUserRole<string>>().HasData(superAdminUserRoles);
        }
    }
}
