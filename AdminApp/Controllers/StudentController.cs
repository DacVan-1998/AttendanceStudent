using Microsoft.AspNetCore.Mvc;

namespace AdminApp.Controllers
{
    public class StudentController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View("~/Pages/Student/Index.cshtml");
        }
    }
}