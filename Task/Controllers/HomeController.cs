using Microsoft.AspNetCore.Mvc;
using Task.Contexts;

namespace Task.Controllers
{
    public class HomeController : Controller
    {

        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
                _context= context;  
        }

        public IActionResult Index()
        {

            List<Slide> slides = _context.Slides.ToList();     
            List<Service> services = _context.Services.ToList();

            HomeViewModel viewModel = new() { 
            
                Slides = slides,
                Services = services
            };
            
            return View(viewModel);
        }
    }
}
