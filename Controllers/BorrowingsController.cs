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
    public class BorrowingsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public BorrowingsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string search, int pageNumber = 1)
        {
            ViewData["Search"] = search;

            if (pageNumber < 1)
            {
                return NotFound();
            }

            var borrowings = await _unitOfWork.BorrowingRepository.GetBorrowingDetails(search, pageNumber);

            if (borrowings.PageNumber != 1 && pageNumber > borrowings.PageCount)
            {
                return NotFound();
            }

            var items = borrowings.Select(b => new BorrowingViewModel
            {
                ID = b.ID,
                MemberNumber = b.Member.MemberNumber,
                BookTitle = b.Book.Title,
                DateBorrow = b.DateBorrow,
                DateReturn = b.DateReturn
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

            var borrowing = await _unitOfWork.BorrowingRepository.GetBorrowingDetailById(id);
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
            var members = await _unitOfWork.MemberRepository.GetAll();
            var books = await _unitOfWork.BookRepository.GetAll();

            ViewBag.Members = new SelectList(members, "ID", "MemberNumber");
            ViewBag.Books = new SelectList(books, "ID", "Title");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("MemberID,BookID,DateBorrow")] BorrowingCreateViewModel item)
        {
            var members = await _unitOfWork.MemberRepository.GetAll();
            var books = await _unitOfWork.BookRepository.GetAll();

            ViewBag.Members = new SelectList(members, "ID", "MemberNumber");
            ViewBag.Books = new SelectList(books, "ID", "Title");

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
                    _unitOfWork.BorrowingRepository.Add(borrowing);

                    borrowing.Book.Quantity -= 1;

                    await _unitOfWork.SaveChangesAsync();

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
            var members = await _unitOfWork.MemberRepository.GetAll();
            var books = await _unitOfWork.BookRepository.GetAll();

            ViewBag.Members = new SelectList(members, "ID", "MemberNumber");
            ViewBag.Books = new SelectList(books, "ID", "Title");

            if (id == null)
            {
                return NotFound();
            }

            var borrowing = await _unitOfWork.BorrowingRepository.GetBorrowingDetailById(id);
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
            var members = await _unitOfWork.MemberRepository.GetAll();
            var books = await _unitOfWork.BookRepository.GetAll();

            ViewBag.Members = new SelectList(members, "ID", "MemberNumber");
            ViewBag.Books = new SelectList(books, "ID", "Title");

            if (id != item.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var borrowing = await _unitOfWork.BorrowingRepository.GetBorrowingDetailById(id);
                    borrowing.MemberID = item.MemberID;
                    borrowing.BookID = item.BookID;
                    borrowing.DateReturn = item.DateReturn;
                    borrowing.UpdatedAt = DateTime.UtcNow;
                    _unitOfWork.BorrowingRepository.Update(borrowing);

                    borrowing.Book.Quantity += 1;

                    await _unitOfWork.SaveChangesAsync();

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
            var borrowing = await _unitOfWork.BorrowingRepository.GetBorrowingDetailById(id);
            if (borrowing != null)
            {
                _unitOfWork.BorrowingRepository.Remove(borrowing);

                if (borrowing.DateReturn == null)
                {
                    borrowing.Book.Quantity += 1;
                }
            }

            await _unitOfWork.SaveChangesAsync();

            TempData["Message"] = "The borrowing has been deleted.";

            return RedirectToAction(nameof(Index));
        }
    }
}