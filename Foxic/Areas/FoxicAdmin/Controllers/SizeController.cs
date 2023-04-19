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
	public class SizeController : Controller
    {
        private readonly FoxicDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SizeController(FoxicDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index()
        {
            IEnumerable<Size> sizes = _context.Sizes.AsEnumerable();
            return View(sizes);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Size newsize)
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
            bool isDuplicated = _context.Sizes.Any(s => s.Name == newsize.Name);
            if (isDuplicated)
            {
                ModelState.AddModelError("", "You cannot duplicate value");
                return View();
            }
            _context.Sizes.Add(newsize);
            _context.SaveChanges();


            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            if (id == 0) return NotFound();
            Size size = _context.Sizes.FirstOrDefault(s => s.Id == id);
            if (size is null) return NotFound();
            return View(size);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Edit(int id, Size edited)
        {
            if (id != edited.Id) return BadRequest();
            Size size = _context.Sizes.FirstOrDefault(s => s.Id == id);
            if (size is null) return NotFound();
            bool duplicate = _context.Sizes.Any(s => s.Name == edited.Name && edited.Name != size.Name);
            if (duplicate)
            {
                ModelState.AddModelError("", "You cannot duplicate category name");
                return View(size);
            }
            size.Name = edited.Name;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Delete(int id)
        {
            if (id == 0) return NotFound();
            Size size = _context.Sizes.FirstOrDefault(s => s.Id == id);
            if (size is null) return NotFound();
            return View(size);
        }


        [HttpPost]
        public IActionResult Delete(int id, Size delete)
        {
            if (id != delete.Id) return BadRequest();
            Size size = _context.Sizes.FirstOrDefault(s => s.Id == id);
            if (size is null) return NotFound();
            delete = _context.Sizes.FirstOrDefault(s => s.Id == id);
            if (delete is null) return NotFound();
            _context.Sizes.Remove(delete);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int id)
        {
            if (id <= 0) return NotFound();
            Size sizes = _context.Sizes.FirstOrDefault(c => c.Id == id);
            if (sizes is null) return NotFound();
            return View(sizes);
        }
    }
}
