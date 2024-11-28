using ELibrary.Repositories;
using ELibrary.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ELibrary.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var username = User.Identity?.Name;

            var staff = await _unitOfWork.StaffRepository.GetStaffByUsername(username);
            if (staff == null)
            {
                return NotFound();
            }

            var item = new HomeViewModel
            {
                Name = staff.Name,
            };

            return View(item);
        }
    }
}