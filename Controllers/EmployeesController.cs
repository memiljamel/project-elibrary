using ELibrary.Data;
using ELibrary.Models;
using ELibrary.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList.EF;
using BC = BCrypt.Net.BCrypt;

namespace ELibrary.Controllers
{
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
                query = query.Where(s => s.EmployeeNumber.ToLower().Contains(search.ToLower()) ||
                                         s.Name.ToLower().Contains(search.ToLower()) ||
                                         s.Username.ToLower().Contains(search.ToLower()));
            }

            var staffs = await query.OrderByDescending(s => s.CreatedAt)
                .Select(s => new EmployeeViewModel
                {
                    ID = s.ID,
                    EmployeeNumber = s.EmployeeNumber,
                    Name = s.Name,
                    AccessLevel = s.AccessLevel,
                    Username = s.Username
                })
                .ToPagedListAsync(pageNumber, 15);

            if (staffs.PageNumber != 1 && pageNumber > staffs.PageCount)
            {
                return NotFound();
            }

            return View(staffs);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staff = await _context.Employees.FirstOrDefaultAsync(s => s.ID == id);
            if (staff == null)
            {
                return NotFound();
            }

            var item = new EmployeeViewModel
            {
                ID = staff.ID,
                EmployeeNumber = staff.EmployeeNumber,
                Name = staff.Name,
                AccessLevel = staff.AccessLevel,
                Username = staff.Username,
                CreatedAt = staff.CreatedAt,
                UpdatedAt = staff.UpdatedAt
            };

            return View(item);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Name,EmployeeNumber,AccessLevel,Username,Password,PasswordConfirmation")]
            EmployeeCreateViewModel item)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var staff = new Employee
                    {
                        EmployeeNumber = item.EmployeeNumber,
                        Name = item.Name,
                        AccessLevel = item.AccessLevel,
                        Username = item.Username,
                        Password = BC.HashPassword(item.Password)
                    };
                    _context.Add(staff);
                    await _context.SaveChangesAsync();

                    TempData["Message"] = "The staff has been created.";

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
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staff = await _context.Employees.FirstOrDefaultAsync(s => s.ID == id);
            if (staff == null)
            {
                return NotFound();
            }

            var item = new EmployeeEditViewModel
            {
                ID = staff.ID,
                EmployeeNumber = staff.EmployeeNumber,
                Name = staff.Name,
                AccessLevel = staff.AccessLevel,
                Username = staff.Username
            };

            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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
                    var staff = await _context.Employees.FirstOrDefaultAsync(s => s.ID == id);
                    if (staff == null)
                    {
                        return NotFound();
                    }
                    
                    staff.EmployeeNumber = item.EmployeeNumber;
                    staff.Name = item.Name;
                    staff.AccessLevel = item.AccessLevel;
                    staff.UpdatedAt = DateTime.UtcNow;

                    if (!string.IsNullOrEmpty(item.Password))
                    {
                        staff.Password = BC.HashPassword(item.Password);
                    }

                    _context.Update(staff);
                    await _context.SaveChangesAsync();

                    TempData["Message"] = "The staff has been updated.";

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
        public async Task<IActionResult> Delete(Guid id)
        {
            var staff = await _context.Employees.FirstOrDefaultAsync(s => s.ID == id);
            if (staff != null)
            {
                _context.Remove(staff);
            }

            await _context.SaveChangesAsync();

            TempData["Message"] = "The staff has been deleted.";

            return RedirectToAction(nameof(Index));
        }
    }
}