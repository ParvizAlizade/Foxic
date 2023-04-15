using Foxic.DAL;
using Foxic.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Foxic.Areas.FoxicAdmin.Controllers
{
    [Area("FoxicAdmin")]
    public class CollectionController : Controller
    {
        private readonly FoxicDbContext _context;

        public CollectionController(FoxicDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            IEnumerable<Collection> collections = _context.Collections.AsEnumerable();
            return View(collections);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(Collection newcollection)
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
            bool isDuplicated = _context.Collections.Any(c => c.Name == newcollection.Name);
            if (isDuplicated)
            {
                ModelState.AddModelError("", "You cannot duplicate value");
                return View();
            }
            _context.Collections.Add(newcollection);
            _context.SaveChanges();


            return RedirectToAction(nameof(Index));
        }


        public IActionResult Edit(int id)
        {
            if (id == 0) return NotFound();
            Collection collection = _context.Collections.FirstOrDefault(c => c.Id == id);
            if (collection is null) return NotFound();
            return View(collection);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Edit(int id, Collection edited)
        {
            if (id != edited.Id) return BadRequest();
            Collection collection = _context.Collections.FirstOrDefault(c => c.Id == id);
            if (collection is null) return NotFound();
            bool duplicate = _context.Collections.Any(c => c.Name == edited.Name && edited.Name != collection.Name);
            if (duplicate)
            {
                ModelState.AddModelError("", "You cannot duplicate collection name");
                return View(collection);
            }
            collection.Name = edited.Name;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }



        public IActionResult Delete(int id)
        {
            if (id == 0) return NotFound();
            Collection collection = _context.Collections.FirstOrDefault(c => c.Id == id);
            if (collection is null) return NotFound();
            return View();
        }


        [HttpPost]
        public IActionResult Delete(int id, Collection delete)
        {
            if (id != delete.Id) return BadRequest();
            Collection collection = _context.Collections.FirstOrDefault(c => c.Id == id);
            if (collection is null) return NotFound();
            delete = _context.Collections.FirstOrDefault(_c => _c.Id == id);
            if (delete is null) return NotFound();
            _context.Collections.Remove(delete);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int id)
        {
            if (id <= 0) return NotFound();
            Collection collection = _context.Collections.FirstOrDefault(c => c.Id == id);
            if (collection is null) return NotFound();
            return View(collection);
        }
    }
}
