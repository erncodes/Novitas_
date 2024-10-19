using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Novitas_Blog.Models.View_Models;

namespace Novitas_Blog.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(CreateUserRequest createUserRequest)
        {
            var user = new IdentityUser
            {
                UserName = createUserRequest.Username,
                Email = createUserRequest.Email,
            };

            var userCreationResult = await _userManager.CreateAsync(user, createUserRequest.Password);
            if (userCreationResult.Succeeded)
            {
                var roleIdentity = _userManager.AddToRoleAsync(user, "User");
                if (roleIdentity.IsCompletedSuccessfully)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }
        [HttpGet]
        public IActionResult Login(string? ReturnUrl)
        {
            if (ReturnUrl != null)
            {
                var model = new LoginRequest
                {
                    ReturnUrl = ReturnUrl,
                };
                return View(model);
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            var loginResult = await _signInManager.PasswordSignInAsync(loginRequest.Username, loginRequest.Password, false, false);
            return loginResult.Succeeded
                ? !string.IsNullOrWhiteSpace(loginRequest.ReturnUrl) ? Redirect(loginRequest.ReturnUrl) : RedirectToAction("Index", "Home")
                : View();
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public async Task<IActionResult> AccessDenied()
        {
            return View();
        }
    }
}
