using ELibrary.Models;
using ELibrary.Repositories;
using ELibrary.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using X.PagedList.Extensions;

namespace ELibrary.Controllers
{
    [Authorize]
    public class BooksController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public BooksController(IUnitOfWork unitOfWork)
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

            var books = await _unitOfWork.BookRepository.GetPagedBooksWithAuthors(
                search,
                pageNumber
            );

            if (books.PageNumber != 1 && pageNumber > books.PageCount)
            {
                return NotFound();
            }

            var items = books.Select(b => new BookViewModel
            {
                ID = b.ID,
                Title = b.Title,
                AuthorNames = string.Join(", ", b.BooksAuthors.Select(ba => ba.Author.Name)),
                Category = b.Category,
                Publisher = b.Publisher,
                Quantity = b.Quantity,
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

            var book = await _unitOfWork.BookRepository.GetBookWithAuthorsById(id);
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
                UpdatedAt = book.UpdatedAt,
            };

            return View(item);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var authors = await _unitOfWork.AuthorRepository.GetAll();

            ViewBag.Authors = new SelectList(authors, "ID", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("ID,Title,AuthorIDs,Category,Publisher,Quantity")] FormBookViewModel item
        )
        {
            var authors = await _unitOfWork.AuthorRepository.GetAll();

            ViewBag.Authors = new SelectList(authors, "ID", "Name");

            if (ModelState.IsValid)
            {
                try
                {
                    var book = new Book
                    {
                        Title = item.Title,
                        Category = item.Category,
                        Publisher = item.Publisher,
                        Quantity = item.Quantity,
                    };
                    _unitOfWork.BookRepository.Add(book);

                    foreach (var authorId in item.AuthorIDs)
                    {
                        var bookAuthor = new BookAuthor { BookID = book.ID, AuthorID = authorId };
                        _unitOfWork.BookAuthorRepository.Add(bookAuthor);
                    }

                    await _unitOfWork.SaveChangesAsync();

                    TempData["Message"] = "The book has been created.";

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
        public async Task<IActionResult> Edit(Guid? id)
        {
            var authors = await _unitOfWork.AuthorRepository.GetAll();

            ViewBag.Authors = new SelectList(authors, "ID", "Name");

            if (id == null)
            {
                return NotFound();
            }

            var book = await _unitOfWork.BookRepository.GetBookWithAuthorsById(id);
            if (book == null)
            {
                return NotFound();
            }

            var item = new FormBookViewModel
            {
                ID = book.ID,
                Title = book.Title,
                AuthorIDs = book.BooksAuthors.Select(ba => ba.AuthorID),
                Category = book.Category,
                Publisher = book.Publisher,
                Quantity = book.Quantity,
            };

            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            Guid id,
            [Bind("ID,Title,AuthorIDs,Category,Publisher,Quantity")] FormBookViewModel item
        )
        {
            var authors = await _unitOfWork.AuthorRepository.GetAll();

            ViewBag.Authors = new SelectList(authors, "ID", "Name");

            if (id != item.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var book = await _unitOfWork.BookRepository.GetBookWithAuthorsById(id);
                    if (book != null)
                    {
                        _unitOfWork.BookAuthorRepository.RemoveRange(book.BooksAuthors);

                        book.Title = item.Title;
                        book.Category = item.Category;
                        book.Publisher = item.Publisher;
                        book.Quantity = item.Quantity;
                        book.UpdatedAt = DateTime.UtcNow;

                        foreach (var authorId in item.AuthorIDs)
                        {
                            var bookAuthor = new BookAuthor
                            {
                                BookID = book.ID,
                                AuthorID = authorId,
                            };
                            _unitOfWork.BookAuthorRepository.Add(bookAuthor);
                        }
                    }

                    await _unitOfWork.SaveChangesAsync();

                    TempData["Message"] = "The book has been updated.";

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var book = await _unitOfWork.BookRepository.GetBookWithAuthorsById(id);
            if (book != null)
            {
                _unitOfWork.BookAuthorRepository.RemoveRange(book.BooksAuthors);
                _unitOfWork.BookRepository.Remove(book);
            }

            await _unitOfWork.SaveChangesAsync();

            TempData["Message"] = "The book has been deleted.";

            return RedirectToAction(nameof(Index));
        }
    }
}
