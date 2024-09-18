using ELibrary.Models;
using ELibrary.Repositories;
using ELibrary.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList.Extensions;
using BC = BCrypt.Net.BCrypt;

namespace ELibrary.Controllers
{
    [Authorize]
    public class StaffsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public StaffsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? search, int pageNumber = 1)
        {
            ViewData["Search"] = search;

            if (pageNumber < 1)
            {
                return NotFound();
            }

            var staffs = await _unitOfWork.StaffRepository.GetPagedStaffs(search, pageNumber);

            if (staffs.PageNumber != 1 && pageNumber > staffs.PageCount)
            {
                return NotFound();
            }

            var items = staffs.Select(s => new StaffViewModel
            {
                ID = s.ID,
                StaffNumber = s.StaffNumber,
                Name = s.Name,
                AccessLevel = s.AccessLevel,
                Username = s.Username,
            });

            return View(items);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staff = await _unitOfWork.StaffRepository.GetById(id);
            if (staff == null)
            {
                return NotFound();
            }

            var item = new StaffViewModel
            {
                ID = staff.ID,
                StaffNumber = staff.StaffNumber,
                Name = staff.Name,
                AccessLevel = staff.AccessLevel,
                Username = staff.Username,
                CreatedAt = staff.CreatedAt,
                UpdatedAt = staff.UpdatedAt,
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
            [Bind("StaffNumber,Name,AccessLevel,Username,Password,PasswordConfirmation")]
                CreateStaffViewModel item
        )
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var staff = new Staff
                    {
                        StaffNumber = item.StaffNumber,
                        Name = item.Name,
                        AccessLevel = item.AccessLevel,
                        Username = item.Username,
                        Password = BC.HashPassword(item.Password),
                    };
                    _unitOfWork.StaffRepository.Add(staff);

                    await _unitOfWork.SaveChangesAsync();

                    TempData["Message"] = "The staff has been created.";

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        "Unable to save changes. "
                            + "Try again, and if the problem persists "
                            + "see your system administrator."
                    );
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

            var staff = await _unitOfWork.StaffRepository.GetById(id);
            if (staff == null)
            {
                return NotFound();
            }

            var item = new EditStaffViewModel
            {
                ID = staff.ID,
                StaffNumber = staff.StaffNumber,
                Name = staff.Name,
                AccessLevel = staff.AccessLevel,
                Username = staff.Username,
            };

            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(
            Guid id,
            [Bind("ID,StaffNumber,Name,AccessLevel,Username,Password,PasswordConfirmation")]
                EditStaffViewModel item
        )
        {
            if (id != item.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var staff = await _unitOfWork.StaffRepository.GetById(id);
                    if (staff != null)
                    {
                        staff.StaffNumber = item.StaffNumber;
                        staff.Name = item.Name;
                        staff.AccessLevel = item.AccessLevel;
                        staff.Username = item.Username;
                        staff.UpdatedAt = DateTime.UtcNow;

                        if (!string.IsNullOrEmpty(item.Password))
                        {
                            staff.Password = BC.HashPassword(item.Password);
                        }
                    }

                    await _unitOfWork.SaveChangesAsync();

                    TempData["Message"] = "The staff has been updated.";

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        "Unable to save changes. "
                            + "Try again, and if the problem persists, "
                            + "see your system administrator."
                    );
                }
            }

            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var staff = await _unitOfWork.StaffRepository.GetById(id);
            if (staff != null)
            {
                _unitOfWork.StaffRepository.Remove(staff);
            }

            await _unitOfWork.SaveChangesAsync();

            TempData["Message"] = "The staff has been deleted.";

            return RedirectToAction(nameof(Index));
        }
    }
}
