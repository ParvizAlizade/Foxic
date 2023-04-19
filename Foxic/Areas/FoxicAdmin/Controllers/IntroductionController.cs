using Foxic.DAL;
using Foxic.Entities;
using Foxic.Utilities.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Foxic.Areas.FoxicAdmin.Controllers
{
    [Area("FoxicAdmin")]
	[Authorize(Roles = "Admin, Moderator")]
	public class IntroductionController : Controller
    {
        private readonly FoxicDbContext _context;
        private readonly IWebHostEnvironment _env;

        public IntroductionController(FoxicDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            IEnumerable<Introduction> introductions = _context.Introductions.AsEnumerable();
            return View(introductions);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Introduction newintro)
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
            bool isDuplicated = _context.Introductions.Any(i => i.DryClean == newintro.DryClean && i.Polyester==newintro.Polyester && i.Chlorine==newintro.Chlorine && i.Lining==newintro.Lining );
            if (isDuplicated)
            {
                ModelState.AddModelError("", "You cannot duplicate value");
                return View();
            }
            _context.Introductions.Add(newintro);
            _context.SaveChanges();


            return RedirectToAction(nameof(Index));
        }


        public IActionResult Edit(int id)
        {
            if (id == 0) return NotFound();
            Introduction introduction = _context.Introductions.FirstOrDefault(c => c.Id == id);
            if (introduction is null) return NotFound();
            return View(introduction);
        }



        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Edit(int id, Introduction edited)
        {
            if (id != edited.Id) return BadRequest();
            Introduction introduction = _context.Introductions.FirstOrDefault(c => c.Id == id);
            if (introduction is null) return NotFound();

            bool duplicate = _context.Introductions.Any(i => i.DryClean == edited.DryClean && i.Polyester == edited.Polyester && i.Chlorine == edited.Chlorine && i.Lining == edited.Lining);

            if (duplicate)
            {
                ModelState.AddModelError("", "You cannot duplicate same values,If You Dont Want To Change something please click back to list");
                return View(introduction);
            }
            introduction.Polyester = edited.Polyester;
            introduction.Chlorine = edited.Chlorine;
            introduction.Lining = edited.Lining;
            introduction.DryClean = edited.DryClean;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }




        public IActionResult Delete(int id)
        {
            if (id == 0) return NotFound();
            Introduction introduction = _context.Introductions.FirstOrDefault(i => i.Id == id);
            if (introduction is null) return NotFound();
            return View(introduction);
        }


        [HttpPost]
        public IActionResult Delete(int id, Introduction delete)
        {
            if (id != delete.Id) return BadRequest();
            Introduction ?introduction = _context.Introductions.FirstOrDefault(i => i.Id == id);
            if (introduction is null) return NotFound();
            delete = _context.Introductions.FirstOrDefault(i => i.Id == id);
            if (delete is null) return NotFound();
            _context.Introductions.Remove(delete);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Details(int id)
        {
            if (id <= 0) return NotFound();
            Introduction introduction = _context.Introductions.FirstOrDefault(c => c.Id == id);
            if (introduction is null) return NotFound();
            return View(introduction);
        }
    }
}
