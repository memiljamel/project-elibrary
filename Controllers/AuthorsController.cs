using ELibrary.Data;
using ELibrary.Models;
using ELibrary.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList.EF;

namespace ELibrary.Controllers
{
    [Authorize]
    public class AuthorsController : Controller
    {
        private readonly ELibraryContext _context;

        public AuthorsController(ELibraryContext context)
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

            var query = _context.Authors
                .Include(a => a.BooksAuthors)
                .AsQueryable();
            
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(a => a.Name.ToLower().Contains(search.ToLower()) ||
                                         a.Email.ToLower().Contains(search.ToLower()));
            }

            var authors = await query.OrderByDescending(a => a.CreatedAt)
                .Select(a => new AuthorViewModel
                {
                    ID = a.ID,
                    Name = a.Name,
                    Email = a.Email,
                    BookCount = a.BooksAuthors.Count
                })
                .ToPagedListAsync(pageNumber, 15);
            
            if (authors.PageNumber != 1 && pageNumber > authors.PageCount)
            {
                return NotFound();
            }
            
            return View(authors);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await _context.Authors
                .Include(a => a.BooksAuthors)
                .FirstOrDefaultAsync(a => a.ID == id);
            if (author == null)
            {
                return NotFound();
            }

            var item = new AuthorViewModel
            {
                ID = author.ID,
                Name = author.Name,
                Email = author.Email,
                BookCount = author.BooksAuthors.Count,
                CreatedAt = author.CreatedAt,
                UpdatedAt = author.UpdatedAt
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
            [Bind("ID,Name,Email")] AuthorFormViewModel item)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var author = new Author
                    {
                        Name = item.Name,
                        Email = item.Email
                    };
                    _context.Add(author);
                    await _context.SaveChangesAsync();

                    TempData["Message"] = "The author has been created.";

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

            var author = await _context.Authors.FirstOrDefaultAsync(a => a.ID == id);
            if (author == null)
            {
                return NotFound();
            }

            var item = new AuthorFormViewModel
            {
                ID = author.ID,
                Name = author.Name,
                Email = author.Email
            };
            
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            Guid id, 
            [Bind("ID,Name,Email")] AuthorFormViewModel item)
        {
            if (id != item.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var author = await _context.Authors.FirstOrDefaultAsync(a => a.ID == id);
                    if (author == null)
                    {
                        return NotFound();
                    }

                    author.Name = item.Name;
                    author.Email = item.Email;
                    author.UpdatedAt = DateTime.UtcNow;
                    _context.Update(author);
                    await _context.SaveChangesAsync();

                    TempData["Message"] = "The author has been updated.";
                    
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
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
            var author = await _context.Authors.FirstOrDefaultAsync(a => a.ID == id);
            if (author != null)
            {
                _context.Authors.Remove(author);
            }

            await _context.SaveChangesAsync();

            TempData["Message"] = "The author has been deleted.";

            return RedirectToAction(nameof(Index));
        }
    }
}
