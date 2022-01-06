using Microsoft.AspNetCore.Mvc;

namespace AdminApp.Controllers
{
    public class ClassController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View("~/Pages/Class/Index.cshtml");
        }
    }
}