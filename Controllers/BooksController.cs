using ELibrary.Data;
using ELibrary.Models;
using ELibrary.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using X.PagedList.EF;

namespace ELibrary.Controllers
{
    [Authorize]
    public class BooksController : Controller
    {
        private readonly ELibraryContext _context;

        public BooksController(ELibraryContext context)
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
            
            var query = _context.Books
                .Include(b => b.BooksAuthors)
                .ThenInclude(ba => ba.Author)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(b => b.Title.ToLower().Contains(search.ToLower()) ||
                                         b.BooksAuthors.Any(ba => ba.Author.Name.ToLower().Contains(search.ToLower())) ||
                                         b.Publisher.ToLower().Contains(search.ToLower()) ||
                                         b.Quantity.ToString().ToLower().Contains(search.ToLower()));
            }
            
            var books = await query.OrderByDescending(b => b.CreatedAt)
                .Select(b => new BookViewModel
                {
                    ID = b.ID,
                    Title = b.Title,
                    AuthorNames = string.Join(", ", b.BooksAuthors.Select(ba => ba.Author.Name)),
                    Category = b.Category,
                    Publisher = b.Publisher,
                    Quantity = b.Quantity
                })
                .ToPagedListAsync(pageNumber, 15);
            
            if (books.PageNumber != 1 && pageNumber > books.PageCount)
            {
                return NotFound();
            }

            return View(books);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.BooksAuthors)
                .ThenInclude(ba => ba.Author)
                .FirstOrDefaultAsync(b => b.ID == id);
            if (book == null)
            {
                return NotFound();
            }

            var item = new BookViewModel
            {
                ID = book.ID,
                Title = book.Title,
                AuthorNames = string.Join(", ", book.BooksAuthors.Select(ba => ba.Author.Name)),
                Category = book.Category,
                Publisher = book.Publisher,
                Quantity = book.Quantity,
                CreatedAt = book.CreatedAt,
                UpdatedAt = book.UpdatedAt
            };

            return View(item);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Authors = new SelectList(await _context.Authors.ToListAsync(), "ID", "Name");
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("ID,Title,AuthorIDs,Category,Publisher,Quantity")] 
            BookFormViewModel item)
        {
            ViewBag.Authors = new SelectList(await _context.Authors.ToListAsync(), "ID", "Name");
            
            if (ModelState.IsValid)
            {
                try
                {
                    var book = new Book
                    {
                        Title = item.Title,
                        Category = item.Category,
                        Publisher = item.Publisher,
                        Quantity = item.Quantity
                    };
                    _context.Add(book);

                    foreach (var authorId in item.AuthorIDs)
                    {
                        var bookAuthor = new BookAuthor
                        {
                            BookID = book.ID,
                            AuthorID = authorId,
                        };
                        _context.Add(bookAuthor);
                    }
                    
                    await _context.SaveChangesAsync();
                    
                    TempData["Message"] = "The book has been created.";
                
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
            ViewBag.Authors = new SelectList(await _context.Authors.ToListAsync(), "ID", "Name");
            
            if (id == null)
            {
                return NotFound();
            }
            
            var book = await _context.Books
                .Include(b => b.BooksAuthors)
                .ThenInclude(ba => ba.Author)
                .FirstOrDefaultAsync(b => b.ID == id);
            if (book == null)
            {
                return NotFound();
            }

            var item = new BookFormViewModel
            {
                ID = book.ID,
                Title = book.Title,
                AuthorIDs = book.BooksAuthors.Select(ba => ba.AuthorID).ToList(),
                Category = book.Category,
                Publisher = book.Publisher,
                Quantity = book.Quantity
            };
            
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            Guid id,
            [Bind("ID,Title,AuthorIDs,Category,Publisher,Quantity")]
            BookFormViewModel item)
        {
            ViewBag.Authors = new SelectList(await _context.Authors.ToListAsync(), "ID", "Name");
            
            if (id != item.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var book = await _context.Books
                        .Include(b => b.BooksAuthors)
                        .ThenInclude(ba => ba.Author)
                        .FirstOrDefaultAsync(b => b.ID == id);
                    if (book == null)
                    {
                        return NotFound();
                    }
                    
                    book.BooksAuthors.Clear();
                    await _context.SaveChangesAsync();

                    book.Title = item.Title;
                    book.Category = item.Category;
                    book.Publisher = item.Publisher;
                    book.Quantity = item.Quantity;
                    book.UpdatedAt = DateTime.UtcNow;
                    _context.Update(book);
                    
                    foreach (var authorId in item.AuthorIDs)
                    {
                        var bookAuthor = new BookAuthor
                        {
                            BookID = book.ID,
                            AuthorID = authorId,
                        };
                        _context.Add(bookAuthor);
                    }
                    
                    await _context.SaveChangesAsync();
                    
                    TempData["Message"] = "The book has been updated.";

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
            var book = await _context.Books
                .Include(b => b.BooksAuthors)
                .ThenInclude(ba => ba.Author)
                .FirstOrDefaultAsync(b => b.ID == id);
            if (book != null)
            {
                book.BooksAuthors.Clear();
                _context.Remove(book);
            }
            
            await _context.SaveChangesAsync();

            TempData["Message"] = "The book has been deleted.";

            return RedirectToAction(nameof(Index));
        }
    }
}
