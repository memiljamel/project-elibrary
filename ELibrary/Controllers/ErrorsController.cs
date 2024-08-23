using ELibrary.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ELibrary.Controllers
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class ErrorsController : Controller
    {
        [ActionName("403")]
        public IActionResult Error403()
        {
            var item = new ErrorViewModel
            {
                StatusCode = 403,
                Message = "Forbidden"
            };
            
            return View("Oops", item);
        }
        
        [ActionName("404")]
        public IActionResult Error404()
        {
            var item = new ErrorViewModel
            {
                StatusCode = 404,
                Message = "Not Found"
            };
            
            return View("Oops", item);
        }
        
        [ActionName("500")]
        public IActionResult Error500()
        {
            var item = new ErrorViewModel
            {
                StatusCode = 500,
                Message = "Internal Server Error"
            };
            
            return View("Oops", item);
        }
    }
}