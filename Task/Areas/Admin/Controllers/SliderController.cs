using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task.Areas.Admin.ViewModels;
using Task.Contexts;

namespace Task.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderController : Controller
    {

        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment; // wwwroot`a qədər olan hissəni dinamikləşdirmək

        public SliderController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {

            List<Slide> slides = _context.Slides.ToList();

            return View(slides);
        }

        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(SlideViewModel slideViewModel) // Async Actions
        {

            if (!ModelState.IsValid) // Check annotation is valid
            {
                return View();
            }

            if (slideViewModel.Offer > 100)
            {
                ModelState.AddModelError("Offer", "More than 100"); //Error message 
                return View();

            }

            if (slideViewModel.Image.Length / 1024 > 100) // Check Size of File (Must be <100)
            {
                ModelState.AddModelError("Image", "Image size is not correct");
                return View();
            }

            if (!slideViewModel.Image.ContentType.Contains("image/")) // Check Type of File (Must be image)
            {
                ModelState.AddModelError("Image", "File type is not correct");
                return View();
            }

            
            string filename = Guid.NewGuid().ToString()+ "-" + slideViewModel.Image.FileName; // Creating Unique image name 

            //string path = _webHostEnvironment.WebRootPath + @"\assets\images\website-images\" + filename; // Path for save images
            string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "images", "website-images", filename); // Əməliyyat sistemi fərqliliyənə görə uyğunlaşdırmaq


            using (FileStream stream = new FileStream(path , FileMode.Create)) // File`n path`ni veririk, və Path yaratdığımız üçün create edirik
            {
                await slideViewModel.Image.CopyToAsync(stream); //
            }


            Slide slide = new Slide()
            {
                Image = filename,
                Offer = slideViewModel.Offer,
                Title= slideViewModel.Title,    
                Description= slideViewModel.Description,    
            };

            //return Content(slideViewModel.Image.FileName); // Create.cshtml`dƏ uöload olunan File`n Name`ni çıxardır
            //return Content(slideViewModel.Image.Length.ToString(); // File`n ölçüsünü verir
            //return Content(slideViewModel.Image.FileName); //Type `nı verir məsələn İmage Application Text

            _context.Slides.AddAsync(slide); // Add slide`ı to table
            _context.SaveChangesAsync(); // Save table to Database

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Detail(int id)
        {
            Slide? slider = _context.Slides.FirstOrDefault(s => s.Id == id);

            if (slider == null)
            {
                return NotFound();
            }


            return View(slider);
        }

        public IActionResult Delete(int id)
        {

            if (_context.Slides.Count() == 0)
            {
                return BadRequest();
            }

            Slide? slider = _context.Slides.FirstOrDefault(s => s.Id == id);

            if (slider == null)
            {
                return NotFound();
            }


            return View(slider);
        }

        [HttpPost]
        [ActionName("Delete")]
        public IActionResult DeleteService(int id)
        {

            Slide? slider = _context.Slides.FirstOrDefault(s => s.Id == id);

            if (slider == null)
            {
                return NotFound();
            }

            _context.Slides.Remove(slider);
            _context.SaveChanges();


            return RedirectToAction(nameof(Index));
        }

        public IActionResult Update(int id)
        {
            Slide? slider = _context.Slides.FirstOrDefault(_context => _context.Id == id);
            if (slider == null)
            {
                return NotFound();
            }

            return View(slider);
        }

        [HttpPost]
        public IActionResult Update(Slide serviceItem, int id)
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
            Slide? service = _context.Slides.AsNoTracking().FirstOrDefault(s => s.Id == id);
            if (service is null)
            {
                return NotFound();
            }

            _context.Slides.Update(serviceItem);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }

    }
}
