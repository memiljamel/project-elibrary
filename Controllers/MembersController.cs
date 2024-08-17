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
    public class MembersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public MembersController(IUnitOfWork unitOfWork)
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

            var members = await _unitOfWork.MemberRepository.GetPagedMembersWithPhones(search, pageNumber);
            
            if (members.PageNumber != 1 && pageNumber > members.PageCount)
            {
                return NotFound();
            }
            
            var items = members.Select(m => new MemberViewModel
            {
                ID = m.ID,
                MemberNumber = m.MemberNumber,
                Name = m.Name,
                Email = m.Email,
                PhoneNumbers = string.Join(", ", m.Phones.Select(p => p.PhoneNumber)),
                Address = m.Address
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

            var member = await _unitOfWork.MemberRepository.GetMemberWithPhonesById(id);
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
                        Address = item.Address
                    };
                    _unitOfWork.MemberRepository.Add(member);

                    var phoneNumbers = item.PhoneNumbers.Split(",", StringSplitOptions.RemoveEmptyEntries)
                        .Select(p => p.Trim())
                        .ToList();
                    
                    foreach (var phoneNumber in phoneNumbers)
                    {
                        var phone = new Phone
                        {
                            PhoneNumber = phoneNumber,
                            MemberID = member.ID
                        };
                        _unitOfWork.PhoneRepository.Add(phone);
                    }
                    
                    await _unitOfWork.SaveChangesAsync();

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

            var member = await _unitOfWork.MemberRepository.GetMemberWithPhonesById(id);
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
                    var member = await _unitOfWork.MemberRepository.GetMemberWithPhonesById(id);
                    
                    _unitOfWork.PhoneRepository.RemoveRange(member.Phones);
                    
                    member.MemberNumber = item.MemberNumber;
                    member.Name = item.Name;
                    member.Email = item.Email;
                    member.Address = item.Address;
                    member.UpdatedAt = DateTime.UtcNow;
                    _unitOfWork.MemberRepository.Update(member);
                    
                    var phoneNumbers = item.PhoneNumbers.Split(",", StringSplitOptions.RemoveEmptyEntries)
                        .Select(p => p.Trim())
                        .ToList();

                    foreach (var phoneNumber in phoneNumbers)
                    {
                        var phone = new Phone
                        {
                            PhoneNumber = phoneNumber,
                            MemberID = member.ID
                        };
                        _unitOfWork.PhoneRepository.Add(phone);
                    }
                    
                    await _unitOfWork.SaveChangesAsync();

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
            var member = await _unitOfWork.MemberRepository.GetMemberWithPhonesById(id);
            if (member != null)
            {
                _unitOfWork.PhoneRepository.RemoveRange(member.Phones);
                _unitOfWork.MemberRepository.Remove(member);
            }

            await _unitOfWork.SaveChangesAsync();

            TempData["Message"] = "The member has been deleted.";

            return RedirectToAction(nameof(Index));
        }
    }
}