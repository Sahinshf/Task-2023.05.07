using Microsoft.AspNetCore.Mvc;

namespace Task.Areas.Admin.Controllers
{
    [Area("Admin")] // Must Write
    public class DashboardController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
    }
}
