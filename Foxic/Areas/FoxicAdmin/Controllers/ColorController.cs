using Foxic.DAL;
using Foxic.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Logging;
using Microsoft.CodeAnalysis.Differencing;
using Foxic.Utilities.Extensions;
using System.Drawing;
using Color = Foxic.Entities.Color;

namespace Foxic.Areas.FoxicAdmin.Controllers
{
    [Area("FoxicAdmin")]
    [Authorize(Roles ="Admin,Moderator")]

    public class ColorController : Controller
    {
        private readonly FoxicDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ColorController(FoxicDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index()
        {
            IEnumerable<Color> colors = _context.Colors.AsEnumerable();
            return View(colors);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(Color newcolor)
        {
            bool isDuplicated = _context.Colors.Any(c => c.Name == newcolor.Name);
            if (isDuplicated)
            {
                ModelState.AddModelError("Name", "You cannot duplicate value");
                return View();
            }


            if (newcolor.Image == null)
            {
                ModelState.AddModelError("Image", "Please choose image");
                return View();
            }
            if (!newcolor.Image.IsValidFile("image/"))
            {
                ModelState.AddModelError("Image", "Please choose image type file");
                return View();
            }
            if (!newcolor.Image.IsValidLength(1))
            {
                ModelState.AddModelError("Image", "Image size must  be maximum 1MB");
                return View();
            }

            string imagesFolderPath = Path.Combine(_env.WebRootPath, "assets", "images");
            newcolor.ColorPath = await newcolor.Image.CreateImage(imagesFolderPath, "Colors");
            _context.Colors.Add(newcolor);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

     
        
        public IActionResult Edit(int id)
        {
            if (id == 0) return NotFound();

            Color color = _context.Colors.FirstOrDefault(s => s.Id == id);
            if (color is null) return BadRequest();
            return View(color);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id,Color edited)
        {
            if (id != edited.Id) return BadRequest();
            Color ?color = _context.Colors.FirstOrDefault(c => c.Id == id);
            if (!ModelState.IsValid) return View(color);

            _context.Entry(color).CurrentValues.SetValues(edited);

            if (edited.Image is not null)
            {
                string imagesFolderPath = Path.Combine(_env.WebRootPath, "assets", "images");
                string filePath = Path.Combine(imagesFolderPath, "Colors", color.ColorPath);
                FileUpload.DeleteImage(filePath);
                color.ColorPath = await edited.Image.CreateImage(imagesFolderPath, "Colors");
            }

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            if (id == 0) return NotFound();
            Color color = _context.Colors.FirstOrDefault(c => c.Id == id);
            if (color is null) return NotFound();
            return View();
        }


        [HttpPost]
        public IActionResult Delete(int id, Color delete)
        {
            if (id != delete.Id) return BadRequest();
            Color color = _context.Colors.FirstOrDefault(c => c.Id == id);
            if (color is null) return NotFound();
            delete = _context.Colors.FirstOrDefault(_c => _c.Id == id);
            if (delete is null) return NotFound();
            _context.Colors.Remove(delete);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int id)
        {
            if (id <= 0) return NotFound();
            Color color = _context.Colors.FirstOrDefault(c => c.Id == id);
            if (color is null) return NotFound();
            return View(color);
        }
    }
}
