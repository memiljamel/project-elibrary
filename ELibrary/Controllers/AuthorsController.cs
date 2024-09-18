using ELibrary.Models;
using ELibrary.Repositories;
using ELibrary.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList.Extensions;

namespace ELibrary.Controllers
{
    [Authorize]
    public class AuthorsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public AuthorsController(IUnitOfWork unitOfWork)
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

            var authors = await _unitOfWork.AuthorRepository.GetPagedAuthors(search, pageNumber);

            if (authors.PageNumber != 1 && pageNumber > authors.PageCount)
            {
                return NotFound();
            }

            var items = authors.Select(a => new AuthorViewModel
            {
                ID = a.ID,
                Name = a.Name,
                Email = a.Email,
                BookCount = a.BooksAuthors.Count,
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

            var author = await _unitOfWork.AuthorRepository.GetAuthorWithBooksAuthorsById(id);
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
                UpdatedAt = author.UpdatedAt,
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
        public async Task<IActionResult> Create([Bind("ID,Name,Email")] FormAuthorViewModel item)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var author = new Author { Name = item.Name, Email = item.Email };
                    _unitOfWork.AuthorRepository.Add(author);

                    await _unitOfWork.SaveChangesAsync();

                    TempData["Message"] = "The author has been created.";

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
            if (id == null)
            {
                return NotFound();
            }

            var author = await _unitOfWork.AuthorRepository.GetById(id);
            if (author == null)
            {
                return NotFound();
            }

            var item = new FormAuthorViewModel
            {
                ID = author.ID,
                Name = author.Name,
                Email = author.Email,
            };

            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            Guid id,
            [Bind("ID,Name,Email")] FormAuthorViewModel item
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
                    var author = await _unitOfWork.AuthorRepository.GetById(id);
                    if (author != null)
                    {
                        author.Name = item.Name;
                        author.Email = item.Email;
                        author.UpdatedAt = DateTime.UtcNow;
                    }

                    await _unitOfWork.SaveChangesAsync();

                    TempData["Message"] = "The author has been updated.";

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
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
        public async Task<IActionResult> Delete(Guid id)
        {
            var author = await _unitOfWork.AuthorRepository.GetById(id);
            if (author != null)
            {
                _unitOfWork.AuthorRepository.Remove(author);
            }

            await _unitOfWork.SaveChangesAsync();

            TempData["Message"] = "The author has been deleted.";

            return RedirectToAction(nameof(Index));
        }
    }
}
