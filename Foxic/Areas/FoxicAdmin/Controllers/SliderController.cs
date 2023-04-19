using Foxic.DAL;
using Foxic.Entities;
using Foxic.Utilities.Extensions;
using Foxic.Utilities.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Foxic.Areas.FoxicAdmin.Controllers
{
    [Area("FoxicAdmin")]
	[Authorize(Roles = "Admin, Moderator")]
	public class SliderController : Controller
    {
        private readonly FoxicDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SliderController(FoxicDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            IEnumerable<Slider> sliders = _context.Sliders.AsEnumerable();
            return View(sliders);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Slider newSlider)
        {
            if (!ModelState.IsValid)
            {
                foreach (string message in ModelState.Values.SelectMany(v => v.Errors)
                                    .Select(e => e.ErrorMessage))
                {
                    ModelState.AddModelError("", message);
                }

                return View();
            }

            bool isDuplicatedOrder = _context.Sliders.Any(s => s.Order == newSlider.Order);
            if (isDuplicatedOrder)
            {
                ModelState.AddModelError("Order", "You cannot duplicate Order");
                return View();
            }

            if (newSlider.Image is null)
            {
                ModelState.AddModelError("Image", "Please choose image");
                return View();
            }
            if (!newSlider.Image.IsValidLength(3))
            {
                ModelState.AddModelError("Image","Image must be maximum 3MB");
                return View();
            }
            if (newSlider.Image == null)
            {
                ModelState.AddModelError("Image","Please choose image");
                return View();
            }
            if (!newSlider.Image.IsValidFile("image/"))
            {
                ModelState.AddModelError("Image","Please choose image type file");
                return View();
            }

            string imagesFolderPath = Path.Combine(_env.WebRootPath, "assets", "images");
            newSlider.ImagePath = await newSlider.Image.CreateImage(imagesFolderPath, "products");
            _context.Sliders.Add(newSlider);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }


        public IActionResult Edit(int id)
        {
            if (id == 0) return NotFound();

            Slider slider = _context.Sliders.FirstOrDefault(s => s.Id == id);
            if (slider is null) return BadRequest();
            return View(slider);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Slider edited)
        {
            if (id != edited.Id) return BadRequest();
            Slider ?slider = _context.Sliders.FirstOrDefault(s => s.Id == id);
            if (!ModelState.IsValid) return View(slider);

            _context.Entry(slider).CurrentValues.SetValues(edited);

            
            if (edited.Image is not null)
            {
                
            if (!edited.Image.IsValidLength(1))
            {
                ModelState.AddModelError("Image", "Image must be maximum 1MB");
                return View();
            }
            if (!edited.Image.IsValidFile("image/"))
            {
                ModelState.AddModelError("Image", "Please choose image type file");
                return View();
            }
            if (edited.Image is not null)
            {
                string imagesFolderPath = Path.Combine(_env.WebRootPath, "assets", "images");
                string filePath = Path.Combine(imagesFolderPath, "products", slider.ImagePath);
                FileUpload.DeleteImage(filePath);
                slider.ImagePath = await edited.Image.CreateImage(imagesFolderPath, "products");
            }
            }
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int id)
        {
            if (id <=0) return BadRequest();
            Slider slider=_context.Sliders.FirstOrDefault(s=>s.Id == id);
            if (slider is null) return NotFound();
            return View(slider);  
        }

        public IActionResult Delete(int id)
        {
            if (id == 0) return NotFound();
            Slider slider = _context.Sliders.FirstOrDefault(c => c.Id == id);
            if (slider is null) return NotFound();
            return View(slider);
        }

        [HttpPost]
        public IActionResult Delete(int id, Slider delete)
        {
            if (id != delete.Id) return BadRequest();
            Slider slider = _context.Sliders.FirstOrDefault(c => c.Id == id);
            if (slider is null) return NotFound();
            delete = _context.Sliders.FirstOrDefault(_c => _c.Id == id);
            if (delete is null) return NotFound();
            _context.Sliders.Remove(delete);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
