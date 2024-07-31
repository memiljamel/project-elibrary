using ELibrary.Data;
using ELibrary.Models;
using ELibrary.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList.EF;

namespace ELibrary.Controllers
{
    public class MembersController : Controller
    {
        private readonly ELibraryContext _context;

        public MembersController(ELibraryContext context)
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

            var query = _context.Members.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(m => m.MemberNumber.ToLower().Contains(search.ToLower()) ||
                                         m.Name.ToLower().Contains(search.ToLower()) ||
                                         m.Email.ToLower().Contains(search.ToLower()) ||
                                         m.Phones.Any(p => p.PhoneNumber.Contains(search)) ||
                                         m.Address.ToLower().Contains(search.ToLower()));
            }

            var members = await query.Select(m => new MemberViewModel
                {
                    ID = m.ID,
                    MemberNumber = m.MemberNumber,
                    Name = m.Name,
                    Email = m.Email,
                    PhoneNumbers = string.Join(", ", m.Phones.Select(p => p.PhoneNumber)),
                    Address = m.Address
                })
                .ToPagedListAsync(pageNumber, 15);

            return View(members);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members
                .Include(m => m.Phones)
                .FirstOrDefaultAsync(s => s.ID == id);
            if (member == null)
            {
                return NotFound();
            }

            var item = new MemberViewModel
            {
                ID = member.ID,
                MemberNumber = member.MemberNumber,
                Name = member.Name,
                Email = member.Email,
                PhoneNumbers = string.Join(", ", member.Phones.Select(p => p.PhoneNumber)),
                Address = member.Address,
                CreatedAt = member.CreatedAt,
                UpdatedAt = member.UpdatedAt
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
            [Bind("ID,MemberNumber,Name,Email,PhoneNumbers,Address")]
            MemberFormViewModel item)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var member = new Member
                    {
                        MemberNumber = item.MemberNumber,
                        Name = item.Name,
                        Email = item.Email,
                        Phones = item.PhoneNumbers.Split(",", StringSplitOptions.RemoveEmptyEntries)
                            .Select(p => new Phone { PhoneNumber = p.Trim() })
                            .ToList(),
                        Address = item.Address
                    };
                    _context.Add(member);
                    await _context.SaveChangesAsync();

                    TempData["Message"] = "The member has been created.";

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

            var member = await _context.Members
                .Include(m => m.Phones)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (member == null)
            {
                return NotFound();
            }

            var item = new MemberFormViewModel
            {
                ID = member.ID,
                MemberNumber = member.MemberNumber,
                Name = member.Name,
                Email = member.Email,
                PhoneNumbers = string.Join(", ", member.Phones.Select(p => p.PhoneNumber)),
                Address = member.Address
            };

            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            Guid id,
            [Bind("ID,MemberNumber,Name,Email,PhoneNumbers,Address")]
            MemberFormViewModel item)
        {
            if (id != item.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var member = await _context.Members
                        .Include(m => m.Phones)
                        .FirstOrDefaultAsync(m => m.ID == id);
                    if (member == null)
                    {
                        return NotFound();
                    }
                    
                    member.MemberNumber = item.MemberNumber;
                    member.Name = item.Name;
                    member.Email = item.Email;
                    member.Address = item.Address;

                    member.Phones.Clear();
                    member.Phones = item.PhoneNumbers.Split(",", StringSplitOptions.RemoveEmptyEntries)
                        .Select(p => new Phone { PhoneNumber = p.Trim() })
                        .ToList();

                    _context.Update(member);
                    await _context.SaveChangesAsync();

                    TempData["Message"] = "The member has been updated.";

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var member = await _context.Members
                .Include(m => m.Phones)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (member != null)
            {
                _context.Phones.RemoveRange(member.Phones);
                _context.Remove(member);
            }

            await _context.SaveChangesAsync();

            TempData["Message"] = "The member has been deleted.";

            return RedirectToAction(nameof(Index));
        }
    }
}