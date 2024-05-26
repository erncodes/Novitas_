using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Novitas_Blog.Data;
using Novitas_Blog.Repositories;

namespace Novitas_Blog
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<NovitasDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("NovitasConnectionString")));
            builder.Services.AddDbContext<NovitasAuthDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("NovitasAuthConnectionString")));
            builder.Services.AddScoped<IBlogArticleRepository, BlogArticleRepository>();
            builder.Services.AddScoped<IBlogTagRepository, BlogTagRepository>();
            builder.Services.AddScoped<ICommentRepository, CommentRepository>();
            builder.Services.AddScoped<IImageRepository, ImageRepository>();
            builder.Services.AddScoped<ICategoryRepository,CategoryRepository>();
            builder.Services.AddScoped<IUserRepository,UserRepository>();
            builder.Services.AddIdentity<IdentityUser,IdentityRole>(options =>
            {
                options.Password.RequiredLength = 10;
                options.Password.RequiredUniqueChars = 3;
            })
                .AddEntityFrameworkStores<NovitasAuthDBContext>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}