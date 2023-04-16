using Foxic.DAL;
using Foxic.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Foxic.Areas.FoxicAdmin.Controllers
{
    [Area("FoxicAdmin")]
    public class TagController : Controller
    {
        private readonly FoxicDbContext _context;

        public TagController(FoxicDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            IEnumerable<Tag> tags = _context.Tags.AsEnumerable();
            return View(tags);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(Tag newtag)
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
            bool isDuplicated = _context.Tags.Any(c => c.Name == newtag.Name);
            if (isDuplicated)
            {
                ModelState.AddModelError("", "You cannot duplicate value");
                return View();
            }
            _context.Tags.Add(newtag);
            _context.SaveChanges();


            return RedirectToAction(nameof(Index));
        }


        public IActionResult Edit(int id)
        {
            if (id == 0) return NotFound();
            Tag tag = _context.Tags.FirstOrDefault(t => t.Id == id);
            if (tag is null) return NotFound();
            return View(tag);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Edit(int id, Tag edited)
        {
            if (id != edited.Id) return BadRequest();
            Tag tag = _context.Tags.FirstOrDefault(t => t.Id == id);
            if (tag is null) return NotFound();
            bool duplicate = _context.Tags.Any(t => t.Name == edited.Name && edited.Name != tag.Name);
            if (duplicate)
            {
                ModelState.AddModelError("", "You cannot duplicate tag name");
                return View(tag);
            }
            tag.Name = edited.Name;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }



        public IActionResult Delete(int id)
        {
            if (id == 0) return NotFound();
            Tag tag = _context.Tags.FirstOrDefault(t => t.Id == id);
            if (tag is null) return NotFound();
            return View();
        }


        [HttpPost]
        public IActionResult Delete(int id, Tag delete)
        {
            if (id != delete.Id) return BadRequest();
            Tag tag = _context.Tags.FirstOrDefault(c => c.Id == id);
            if (tag is null) return NotFound();
            delete = _context.Tags.FirstOrDefault(t => t.Id == id);
            if (delete is null) return NotFound();
            _context.Tags.Remove(delete);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int id)
        {
            if (id <= 0) return NotFound();
            Tag tag = _context.Tags.FirstOrDefault(c => c.Id == id);
            if (tag is null) return NotFound();
            return View(tag);
        }
    }
}
