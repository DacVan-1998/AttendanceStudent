using Microsoft.AspNetCore.Mvc;

namespace AdminApp.Controllers
{
    public class StudentImagesController : Controller
    {
        // GET
        public IActionResult Index([FromQuery] string studentId)
        {
            return View("~/Pages/StudentImages/Index.cshtml",studentId);
        }
    }
}