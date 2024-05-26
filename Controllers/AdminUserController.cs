using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Novitas_Blog.Models.View_Models;
using Novitas_Blog.Repositories;

namespace Novitas_Blog.Controllers
{
    public class AdminUserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<IdentityUser> _userManager;

        public AdminUserController(IUserRepository userRepository, UserManager<IdentityUser> userManager)
        {
            this._userRepository = userRepository;
            this._userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _userRepository.GetAllUsers();
            var usersViewmodel = new UserViewModel();
            usersViewmodel.Users = new List<User>();
            if (users != null)
            {
                foreach (var user in users)
                {
                    usersViewmodel.Users.Add(new User
                    {
                        UserId = Guid.Parse(user.Id),
                        Username = user.UserName,
                        Email = user.Email,
                    });
                }
                return View(usersViewmodel);
            }
            return View();
        } 
        [HttpPost]
        public async Task<IActionResult> Index(UserViewModel userViewModel)
        {
            var user = new IdentityUser
            {
                UserName = userViewModel.Username,
                Email = userViewModel.Email,
            };
            var userCreation = await _userManager.CreateAsync(user, userViewModel.Password);
            var roles = new List<string> { "User" };
            if(userCreation != null && userCreation.Succeeded)
            {
                if (userViewModel.IsAdmin && !userViewModel.IsSuperAdmin)
                {
                    roles.Add("Admin");
                }
                else if(userViewModel.IsSuperAdmin && !userViewModel.IsAdmin)
                {
                    roles.Add("SuperAdmin");
                }
                else if(!userViewModel.IsSuperAdmin && userViewModel.IsAdmin)
                {
                    roles.Add("Admin");
                    roles.Add("SuperAdmin");
                }
                var userRoles = new IdentityResult();
                if(roles.Count >= 2)
                {
                    userRoles = await _userManager.AddToRolesAsync(user, roles);
                    if (userRoles.Succeeded)
                    {
                        return RedirectToAction("Index", "AdminUser");
                    }
                }
                else
                {
                    userRoles = await _userManager.AddToRoleAsync(user, roles[0]);
                    if (userRoles.Succeeded)
                    {
                        return RedirectToAction("Index", "AdminUser");
                    }
                }
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Delete(Guid Id)
        {
            var user = await _userManager.FindByIdAsync(Id.ToString());
            if(user != null)
            {
                var userDeletion = await _userManager.DeleteAsync(user);
                if(userDeletion != null && userDeletion.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }
            return View();
        }
    }
}
