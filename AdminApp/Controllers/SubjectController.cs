using Microsoft.AspNetCore.Mvc;

namespace AdminApp.Controllers
{
    public class SubjectController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View("~/Pages/Subject/Index.cshtml");
        }
    }
}