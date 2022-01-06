using System;
using Microsoft.AspNetCore.Mvc;

namespace AdminApp.Controllers
{
    public class RollCallController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View("~/Pages/RollCall/Index.cshtml");
        }
        
        public IActionResult RollCallDetail(Guid rollCallId)
        {
            return View("~/Pages/RollCall/RollCallDetail.cshtml",rollCallId);
        }
    }
}