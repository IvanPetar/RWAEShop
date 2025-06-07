using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using RWAEshopDAL.Services;
using RWAEShopMVC.ViewModels;
using RWAEshopDAL.Models;
using RWAEshopDAL.Security;
using System.Security.Claims;

namespace RWAEShopMVC.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            UserLoginVM loginVM = new UserLoginVM()
            {
                ReturnUrl = returnUrl
            };
            return View();
        }
        [HttpPost]
        public IActionResult Login(UserLoginVM loginVM) 
        {
            var genericLoginFail = "Incorrect username or password";

            // Try to get a user from database
            var existingUser = _userService.GetAllUsers()
                .FirstOrDefault(x => x.Username == loginVM.Username);

            if (existingUser == null)
            {
                ModelState.AddModelError("", genericLoginFail);
                return View();
            }

            // Check is password hash matches
            var b64hash = PasswordHashProvider.GetHash(loginVM.Password, existingUser.PwdSalt);
            if (b64hash != existingUser.PwdHash)
            {
                ModelState.AddModelError("", genericLoginFail);
                return View();
            }

            var claims = new List<Claim>() {
                    new Claim(ClaimTypes.Name, loginVM.Username),
                    new Claim("Name", existingUser.Name),
                    new Claim("LastName", existingUser.LastName),
                    new Claim(ClaimTypes.Role, existingUser.Role.RoleName)
                };

            var claimsIdentity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties();

            Task.Run(async () =>
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties)
            ).GetAwaiter().GetResult();

            if (loginVM.ReturnUrl != null)
                return LocalRedirect(loginVM.ReturnUrl);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Logout()
        {
            Task.Run(async () =>
                await HttpContext.SignOutAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme)
            ).GetAwaiter().GetResult();

            return View();
        }
        //public async Task<IActionResult> Logout()
        //{

        //    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        //    return RedirectToAction("Index", "Home");
        //}

        public IActionResult Register() 
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(UserRegisterVM userVM) 
        {
            try
            {
                // Check if there is such a username in the database already
                var trimmedUsername = userVM.Username.Trim();
                if (_userService.GetAllUsers().Any(x => x.Username.Equals(trimmedUsername)))
                {
                    ModelState.AddModelError("", "Username already exists");
                    return View();
                }

                // Hash the password
                var b64salt = PasswordHashProvider.GetSalt();
                var b64hash = PasswordHashProvider.GetHash(userVM.Password, b64salt);

                // Create user from DTO and hashed password
                var user = _mapper.Map<User>(userVM);
                user.PwdHash = b64hash;
                user.PwdSalt = b64salt;


                user.RoleId = 2;

                // Add user and save changes to database
                _userService.CreateUser(user);


                return RedirectToAction("Login", "User");

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
