using Microsoft.AspNetCore.Mvc;

namespace AdminApp.Controllers
{
    public class AttendanceController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View("~/Pages/Attendance/Index.cshtml");
        }
    }
}