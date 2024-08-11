using ELibrary.Data;
using ELibrary.Models;
using ELibrary.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList.EF;
using BC = BCrypt.Net.BCrypt;

namespace ELibrary.Controllers
{
    [Authorize]
    public class EmployeesController : Controller
    {
        private readonly ELibraryContext _context;

        public EmployeesController(ELibraryContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string search, int pageNumber = 1)
        {
            ViewData["Search"] = search;

            if (pageNumber < 1)
            {
                return NotFound();
            }

            var query = _context.Employees.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(e => e.EmployeeNumber.ToLower().Contains(search.ToLower()) ||
                                         e.Name.ToLower().Contains(search.ToLower()) ||
                                         e.Username.ToLower().Contains(search.ToLower()));
            }

            var employees = await query.OrderByDescending(e => e.CreatedAt)
                .Select(e => new EmployeeViewModel
                {
                    ID = e.ID,
                    EmployeeNumber = e.EmployeeNumber,
                    Name = e.Name,
                    AccessLevel = e.AccessLevel,
                    Username = e.Username
                })
                .ToPagedListAsync(pageNumber, 15);

            if (employees.PageNumber != 1 && pageNumber > employees.PageCount)
            {
                return NotFound();
            }

            return View(employees);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.ID == id);
            if (employee == null)
            {
                return NotFound();
            }

            var item = new EmployeeViewModel
            {
                ID = employee.ID,
                EmployeeNumber = employee.EmployeeNumber,
                Name = employee.Name,
                AccessLevel = employee.AccessLevel,
                Username = employee.Username,
                CreatedAt = employee.CreatedAt,
                UpdatedAt = employee.UpdatedAt
            };

            return View(item);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create(
            [Bind("Name,EmployeeNumber,AccessLevel,Username,Password,PasswordConfirmation")]
            EmployeeCreateViewModel item)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var employee = new Employee
                    {
                        EmployeeNumber = item.EmployeeNumber,
                        Name = item.Name,
                        AccessLevel = item.AccessLevel,
                        Username = item.Username,
                        Password = BC.HashPassword(item.Password)
                    };
                    _context.Add(employee);
                    await _context.SaveChangesAsync();

                    TempData["Message"] = "The employee has been created.";

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError(string.Empty, "Unable to save changes. " +
                                                           "Try again, and if the problem persists " +
                                                           "see your system administrator.");
                }
            }

            return View(item);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.ID == id);
            if (employee == null)
            {
                return NotFound();
            }

            var item = new EmployeeEditViewModel
            {
                ID = employee.ID,
                EmployeeNumber = employee.EmployeeNumber,
                Name = employee.Name,
                AccessLevel = employee.AccessLevel,
                Username = employee.Username
            };

            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(
            Guid id,
            [Bind("ID,Name,EmployeeNumber,AccessLevel,Username,Password,PasswordConfirmation")]
            EmployeeEditViewModel item)
        {
            if (id != item.ID)
            {
                return NotFound();
            }
            
            if (ModelState.IsValid)
            {
                try
                {
                    var employee = await _context.Employees.FirstOrDefaultAsync(e => e.ID == id);
                    if (employee == null)
                    {
                        return NotFound();
                    }
                    
                    employee.EmployeeNumber = item.EmployeeNumber;
                    employee.Name = item.Name;
                    employee.AccessLevel = item.AccessLevel;
                    employee.UpdatedAt = DateTime.UtcNow;

                    if (!string.IsNullOrEmpty(item.Password))
                    {
                        employee.Password = BC.HashPassword(item.Password);
                    }

                    _context.Update(employee);
                    await _context.SaveChangesAsync();

                    TempData["Message"] = "The employee has been updated.";

                    return RedirectToAction(nameof(Index));
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.ID == id);
            if (employee != null)
            {
                _context.Remove(employee);
            }

            await _context.SaveChangesAsync();

            TempData["Message"] = "The employee has been deleted.";

            return RedirectToAction(nameof(Index));
        }
    }
}