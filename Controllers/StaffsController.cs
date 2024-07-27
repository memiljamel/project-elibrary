using ELibrary.Data;
using ELibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList.EF;
using BC = BCrypt.Net.BCrypt;

namespace ELibrary.Controllers
{
    public class StaffsController : Controller
    {
        private readonly ELibraryContext _context;

        public StaffsController(ELibraryContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string searchString, int pageNumber = 1)
        {
            ViewData["SearchString"] = searchString;

            if (pageNumber < 1)
            {
                return NotFound();
            }

            var query = _context.Staffs.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(s => s.Username.ToLower().Contains(searchString.ToLower()) || 
                                         s.Name.ToLower().Contains(searchString.ToLower()) || 
                                         s.EmployeeNumber.ToLower().Contains(searchString.ToLower()));
            }

            var staffs = await query.ToPagedListAsync(pageNumber, 15);

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

            var staff = await _context.Staffs.FirstOrDefaultAsync(s => s.ID == id);
            if (staff == null)
            {
                return NotFound();
            }

            return View(staff);
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
            StaffCreateViewModel item)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var staff = new Staff
                    {
                        Username = item.Username,
                        Password = BC.HashPassword(item.Password),
                        Name = item.Name,
                        EmployeeNumber = item.EmployeeNumber,
                        AccessLevel = item.AccessLevel,
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

            var staff = await _context.Staffs.FindAsync(id);
            if (staff == null)
            {
                return NotFound();
            }

            var item = new StaffEditViewModel
            {
                ID = staff.ID,
                Name = staff.Name,
                EmployeeNumber = staff.EmployeeNumber,
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
            StaffEditViewModel item)
        {
            if (id != item.ID)
            {
                return NotFound();
            }
            
            if (ModelState.IsValid)
            {
                try
                {
                    var staff = await _context.Staffs.FindAsync(id);
                    staff.Name = item.Name;
                    staff.EmployeeNumber = item.EmployeeNumber;
                    staff.AccessLevel = item.AccessLevel;

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
            var staff = await _context.Staffs.FindAsync(id);
            if (staff != null)
            {
                _context.Staffs.Remove(staff);
            }

            await _context.SaveChangesAsync();
            
            TempData["Message"] = "The staff has been deleted.";

            return RedirectToAction(nameof(Index));
        }
    }
}