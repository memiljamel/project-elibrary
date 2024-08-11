using ELibrary.Data;
using ELibrary.Models;
using ELibrary.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using X.PagedList.EF;

namespace ELibrary.Controllers
{
    [Authorize]
    public class BorrowingsController : Controller
    {
        private readonly ELibraryContext _context;

        public BorrowingsController(ELibraryContext context)
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

            var query = _context.Borrowings
                .Include(b => b.Member)
                .Include(b => b.Book)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(b => b.Member.MemberNumber.ToLower().Contains(search.ToLower()) ||
                                         b.Book.Title.ToLower().Contains(search.ToLower()) ||
                                         b.DateBorrow.ToString().ToLower().Contains(search.ToLower()) ||
                                         b.DateReturn.ToString().ToLower().Contains(search.ToLower()));
            }

            var borrowings = await query.OrderByDescending(b => b.CreatedAt)
                .Select(b => new BorrowingViewModel
                {
                    ID = b.ID,
                    MemberNumber = b.Member.MemberNumber,
                    BookTitle = b.Book.Title,
                    DateBorrow = b.DateBorrow,
                    DateReturn = b.DateReturn
                })
                .ToPagedListAsync(pageNumber, 15);

            if (borrowings.PageNumber != 1 && pageNumber > borrowings.PageCount)
            {
                return NotFound();
            }

            return View(borrowings);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var borrowing = await _context.Borrowings
                .Include(b => b.Member)
                .Include(b => b.Book)
                .FirstOrDefaultAsync(b => b.ID == id);
            if (borrowing == null)
            {
                return NotFound();
            }

            var item = new BorrowingViewModel
            {
                ID = borrowing.ID,
                MemberNumber = borrowing.Member.MemberNumber,
                BookTitle = borrowing.Book.Title,
                DateBorrow = borrowing.DateBorrow,
                DateReturn = borrowing.DateReturn,
                CreatedAt = borrowing.CreatedAt,
                UpdatedAt = borrowing.UpdatedAt
            };

            return View(item);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Members = new SelectList(await _context.Members.ToListAsync(), "ID", "MemberNumber");
            ViewBag.Books = new SelectList(await _context.Books.ToListAsync(), "ID", "Title");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("MemberID,BookID,DateBorrow")] BorrowingCreateViewModel item)
        {
            ViewBag.Members = new SelectList(await _context.Members.ToListAsync(), "ID", "MemberNumber");
            ViewBag.Books = new SelectList(await _context.Books.ToListAsync(), "ID", "Title");

            if (ModelState.IsValid)
            {
                try
                {
                    var borrowing = new Borrowing
                    {
                        MemberID = item.MemberID,
                        BookID = item.BookID,
                        DateBorrow = item.DateBorrow
                    };
                    _context.Add(borrowing);
                    
                    borrowing.Book.Quantity -= 1;
                    
                    await _context.SaveChangesAsync();

                    TempData["Message"] = "The borrowing has been created.";

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
            ViewBag.Members = new SelectList(await _context.Members.ToListAsync(), "ID", "MemberNumber");
            ViewBag.Books = new SelectList(await _context.Books.ToListAsync(), "ID", "Title");

            if (id == null)
            {
                return NotFound();
            }

            var borrowing = await _context.Borrowings
                .Include(b => b.Member)
                .Include(b => b.Book)
                .FirstOrDefaultAsync(b => b.ID == id);
            if (borrowing == null)
            {
                return NotFound();
            }

            var item = new BorrowingEditViewModel
            {
                ID = borrowing.ID,
                MemberID = borrowing.MemberID,
                MemberNumber = borrowing.Member.MemberNumber,
                BookID = borrowing.BookID,
                DateBorrow = borrowing.DateBorrow,
                DateReturn = borrowing.DateReturn
            };

            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            Guid id,
            [Bind("ID,MemberID,MemberNumber,BookID,DateBorrow,DateReturn")]
            BorrowingEditViewModel item)
        {
            ViewBag.Members = new SelectList(await _context.Members.ToListAsync(), "ID", "MemberNumber");
            ViewBag.Books = new SelectList(await _context.Books.ToListAsync(), "ID", "Title");

            if (id != item.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var borrowing = await _context.Borrowings
                        .Include(b => b.Member)
                        .Include(b => b.Book)
                        .FirstOrDefaultAsync(b => b.ID == id);
                    if (borrowing == null)
                    {
                        return NotFound();
                    }

                    borrowing.MemberID = item.MemberID;
                    borrowing.BookID = item.BookID;
                    borrowing.DateReturn = item.DateReturn;
                    borrowing.UpdatedAt = DateTime.UtcNow;
                    
                    borrowing.Book.Quantity += 1;
                    
                    _context.Update(borrowing);
                    await _context.SaveChangesAsync();

                    TempData["Message"] = "The borrowing has been updated.";

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
            var borrowing = await _context.Borrowings
                .Include(b => b.Member)
                .Include(b => b.Book)
                .FirstOrDefaultAsync(b => b.ID == id);
            if (borrowing != null)
            {
                if (borrowing.DateReturn == null)
                {
                    borrowing.Book.Quantity += 1;
                }
                
                _context.Borrowings.Remove(borrowing);
            }
            
            await _context.SaveChangesAsync();

            TempData["Message"] = "The borrowing has been deleted.";

            return RedirectToAction(nameof(Index));
        }
    }
}