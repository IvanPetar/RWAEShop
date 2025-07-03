using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using RWAEshopDAL.Services;
using RWAEShopMVC.ViewModels;
using RWAEshopDAL.Models;
using RWAEshopDAL.Security;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace RWAEShopMVC.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper, IOrderService orderService)
        {
            _userService = userService;
            _mapper = mapper;
            _orderService = orderService;
        }

        public ActionResult Index()
        {

            var users =
                _userService.GetAllUsers();

            var model = _mapper.Map<List<UserRegisterVM>>(users);
            return View(model);
        }
        [HttpGet]
        public IActionResult Details(int id)
        {
            var user = _userService.GetUser(id);
            if (user == null) return NotFound();

            var model = _mapper.Map<UserRegisterVM>(user);

            var orders = _orderService.GetAllOrders()
                .Where(o => o.UserId == id)
                .ToList();
            var orderVMs = _mapper.Map<List<OrderVM>>(orders);

            
            var viewModel = new UserDetailsPageVM
            {
                User = model,
                Orders = orderVMs
            };

            return View(viewModel);
        }


        public ActionResult Delete(int id)
        {
            var user = _userService.GetUser(id);
            if (user == null) return NotFound();

            var model = _mapper.Map<UserRegisterVM>(user);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int IdUser)
        {
            var user = _userService.GetUser(IdUser);
            if (user == null)
                return NotFound();

       
            var hasOrders = _orderService.GetAllOrders().Any(o => o.UserId == IdUser);

            if (hasOrders)
            {
                ViewBag.DeleteError = "Not possible to delete user while having order!";
                var model = _mapper.Map<UserRegisterVM>(user);
                return View("Delete", model);
            }

            
            _userService.DeleteUser(IdUser);
            TempData["DeleteSuccess"] = "User successfully deleted.";
            return RedirectToAction("Index");
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

            var authProperties = new AuthenticationProperties()
            {
                IsPersistent = false
            };

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

            
            return RedirectToAction("Index", "Home");
        }
       
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


        [HttpGet]
        public IActionResult Profile()
        {
            var username = User.Identity.Name;
            var user = _userService.GetAllUsers().FirstOrDefault(u=> u.Username == username);
            if (user == null) return NotFound();

            var model = new UserProfileVM
            {
                Username = user.Username,
                Name = user.Name,
                LastName = user.LastName,
                Phone = user.Phone
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Profile([FromForm] UserProfileVM model)
        {
            if (!ModelState.IsValid)
            {
                var errorList = ModelState
                    .Where(x => x.Value.Errors.Any())
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    );

                return BadRequest(new
                {
                    success = false,
                    message = "Validation failed.",
                    errors = errorList
                });
            }
            try
            {
                var username = User.Identity.Name;
                var user = _userService.GetAllUsers().FirstOrDefault(u => u.Username == username);
                if (user == null)
                {
                    return NotFound(new { success = false, message = "User not found." });
                }

                user.Name = model.Name;
                user.LastName = model.LastName;
                user.Phone = model.Phone;

                if (!string.IsNullOrWhiteSpace(model.Password))
                {
                    var salt = PasswordHashProvider.GetSalt();
                    var hash = PasswordHashProvider.GetHash(model.Password, salt);
                    user.PwdSalt = salt;
                    user.PwdHash = hash;
                }

                _userService.UpdateUser(user);

                return Json(new
                {
                    success = true,
                    message = "Profile updated successfully."
                });
            }
            catch (Exception ex) 
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Server error: " + ex.Message,
                    details = ex.ToString()
                });
            }
        }

       
    }
}
