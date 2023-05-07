using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Task.Contexts;

namespace Task.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class ServiceController : Controller
    {

        private readonly AppDbContext _context;
        public ServiceController(AppDbContext context)
        {
            _context= context;  
        }
        
        public IActionResult Index()
        {
            List<Service> services = _context.Services.ToList();    
         
            ViewBag.count = services.Count; 
            
            return View(services);
        }


        public IActionResult Create() { 

            return View();  
        }

        [HttpPost]
        public IActionResult Create(Service service )
        {

            if (_context.Services.Count() > 2)
            {
                return BadRequest();
            }

            List<Service> services = _context.Services.ToList();
            foreach (var item in services)
            {
                if (item.Name == service.Name)
                {
                    return NotFound();

                }
            }

            _context.Services.Add(service); 
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Detail(int id) { 
        
            //_context.Services.Find(id);
            Service? service = _context.Services.FirstOrDefault(s => s.Id == id);
            if (service is null)
            {
                return NotFound();
            }

            return View(service);
        }

        public IActionResult Delete(int id) {

            if (_context.Services.Count()==1)
            {
                return BadRequest();
            }

            Service? service = _context.Services.FirstOrDefault(s => s.Id == id);
            if (service is null)
            {
                return NotFound();
            }

            return  View(service);
        }

        [HttpPost]
        [ActionName("Delete")]
        public IActionResult DeleteService(int id)
        {

            Service? service = _context.Services.FirstOrDefault(s => s.Id == id);
            if (service is null)
            {
                return NotFound();
            }

            _context.Services.Remove(service);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Update(int id)
        {
            Service? service = _context.Services.FirstOrDefault(s => s.Id == id);
            if (service is null)
            {
                return NotFound();
            }


            return View(service);

        }

        [HttpPost]
        public IActionResult Update(Service serviceItem , int id)
        {

            // First Method
            //Service? service = _context.Services.FirstOrDefault(s => s.Id == id);
            //if (service is null)
            //{
            //    return NotFound();
            //}

            //service.Name = serviceItem.Name;    
            //service.Description = serviceItem.Description;  
            //service.Image = serviceItem.Image;

            //Second way
            Service? service = _context.Services.AsNoTracking().FirstOrDefault(s => s.Id == id);
            if (service is null)
            {
                return NotFound();
            }

            List<Service> services = _context.Services.ToList();
            foreach (var item in services)
            {
                if (item.Name == service.Name)
                {
                    return NotFound();

                }
            }

            _context.Services.Update(serviceItem);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }
    }
}
