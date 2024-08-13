using System.Security.Claims;
using ELibrary.Data;
using ELibrary.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BC = BCrypt.Net.BCrypt;

namespace ELibrary.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly ELibraryContext _context;

        public AccountController(ELibraryContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(
            [Bind("Username,Password,IsRemembered")] LoginViewModel item, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                var user = await _context.Employees.FirstOrDefaultAsync(e => e.Username == item.Username);
                
                if (user == null || !BC.Verify(item.Password, user.Password))
                {
                    ModelState.AddModelError(nameof(item.Username), "Invalid login attempt.");

                    return View(item);
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.AccessLevel.ToString())
                };
                
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                    IsPersistent = item.RememberMe,
                    IssuedUtc = DateTimeOffset.UtcNow,
                    RedirectUri = "/Home/Index"
                };
                
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme, 
                    new ClaimsPrincipal(claimsIdentity), 
                    authProperties);

                return Url.IsLocalUrl(returnUrl) 
                    ? Redirect(returnUrl) 
                    : RedirectToAction(nameof(HomeController.Index), "Home");
            }
            
            return View(item);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction(nameof(Login));
        }
        
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var username = User.Identity?.Name;
            
            if (username == null)
            {
                return NotFound();
            }
            
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Username == username);
            if (employee == null)
            {
                return NotFound();
            }
            
            var item = new ProfileViewModel
            {
                EmployeeNumber = employee.EmployeeNumber,
                Name = employee.Name,
                AccessLevel = employee.AccessLevel,
                Username = employee.Username
            };
            
            return View(item);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(
            [Bind("ID,Name,EmployeeNumber,AccessLevel,Username,Password,PasswordConfirmation")] ProfileViewModel item)
        {
            var username = User.Identity?.Name;
            
            if (username == null)
            {
                return NotFound();
            }
            
            if (ModelState.IsValid)
            {
                try
                {
                    var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Username == username);
                    if (employee == null)
                    {
                        return NotFound();
                    }
                    
                    employee.Name = item.Name;
                    employee.UpdatedAt = DateTime.UtcNow;

                    if (!string.IsNullOrEmpty(item.Password))
                    {
                        employee.Password = BC.HashPassword(item.Password);
                    }

                    _context.Update(employee);
                    await _context.SaveChangesAsync();

                    TempData["Message"] = "The profile has been updated.";

                    return RedirectToAction(nameof(Profile));
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError(string.Empty, "Unable to save changes. " +
                                                           "Try again, and if the problem persists, " +
                                                           "see your system administrator.");
                }
            }
            
            return View(item);
        }
    }
}
