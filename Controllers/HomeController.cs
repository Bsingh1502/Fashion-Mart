using FashionMart.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace FashionMart.Controllers
{
    /// <summary>
    /// Controller for handling home page, user authentication, and registration processes.
    /// </summary>
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        /// <param name="userManager">The user manager instance for handling user-related actions.</param>
        /// <param name="signInManager">The sign-in manager instance for handling user sign-ins.</param>
        public HomeController(ILogger<HomeController> logger, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        /// Displays the home page.
        /// </summary>
        /// <returns>The Index view.</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Displays the user registration page.
        /// </summary>
        /// <returns>The Register view.</returns>
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// Handles user registration.
        /// </summary>
        /// <param name="userModel">The model containing user registration data.</param>
        /// <returns>The Index view on success or the Register view with errors on failure.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Register userModel)
        {
            if (!ModelState.IsValid)
            {
                return View(userModel);
            }

            var user = new User()
            {
                FirstName = userModel.FirstName,
                LastName = userModel.LastName,
                UserName = userModel.Email,
                Email = userModel.Email,
            };

            var result = await _userManager.CreateAsync(user, userModel.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return View(userModel);
            }

            var res = await _userManager.AddToRoleAsync(user, "Visitor");
            return View("Index");
        }

        /// <summary>
        /// Handles user login.
        /// </summary>
        /// <param name="userModel">The model containing login data.</param>
        /// <returns>Redirects to the Index view on success or the Login view with errors on failure.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginPost(Login userModel)
        {
            if (!ModelState.IsValid)
            {
                return View(userModel);
            }

            var result = await _signInManager.PasswordSignInAsync(userModel.Email, userModel.Password, userModel.RememberMe, false);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Invalid UserName or Password");
                return View("Login");
            }
        }

        /// <summary>
        /// Displays the login page.
        /// </summary>
        /// <returns>The Login view.</returns>
        [Route("Account/Login")]
        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// Displays the privacy policy page.
        /// </summary>
        /// <returns>The Privacy view.</returns>
        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// Logs out the current user and redirects to the home page.
        /// </summary>
        /// <returns>Redirects to the Index view.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Displays the error page.
        /// </summary>
        /// <returns>The Error view with an error model containing the request ID.</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
