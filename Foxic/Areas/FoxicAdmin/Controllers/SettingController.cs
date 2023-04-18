using Foxic.DAL;
using Foxic.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Foxic.Areas.FoxicAdmin.Controllers
{
    [Area("FoxicAdmin")]
    public class SettingController : Controller
    {
        private readonly FoxicDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SettingController(FoxicDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            IEnumerable<Setting> settings = _context.Settings.AsEnumerable();
            return View(settings);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Setting newsetting)
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
            bool isDuplicated = _context.Settings.Any(i => i.Key == newsetting.Key || i.Value == newsetting.Value);
            if (isDuplicated)
            {
                ModelState.AddModelError("", "You cannot duplicate value");
                return View();
            }
            _context.Settings.Add(newsetting);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }



        public IActionResult Edit(int id)
        {
            if (id == 0) return NotFound();
            Setting? setting = _context.Settings.FirstOrDefault(s => s.Id == id);
            if (setting is null) return NotFound();
            return View(setting);
        }


        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Edit(int id, Setting edited)
        {
            if (id != edited.Id) return BadRequest();
            Setting? setting = _context.Settings.FirstOrDefault(s => s.Id == id);
            if (setting is null) return NotFound();

            bool duplicate = _context.Settings.Any(s => s.Key == edited.Key && edited.Key != setting.Key);


            if (duplicate)
            {
                ModelState.AddModelError("", "You cannot duplicate");
                return View(setting);
            }
            setting.Key = edited.Key;
            setting.Value = edited.Value;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }



        public IActionResult Delete(int id)
        {
            if (id == 0) return NotFound();
            Setting? setting = _context.Settings.FirstOrDefault(s => s.Id == id);
            if (setting is null) return NotFound();
            return View(setting);
        }


        [HttpPost]
        public IActionResult Delete(int id, Setting delete)
        {
            if (id != delete.Id) return BadRequest();
            Setting? setting = _context.Settings.FirstOrDefault(s => s.Id == id);
            if (setting is null) return NotFound();
            delete = _context.Settings.FirstOrDefault(_s => _s.Id == id);
            if (delete is null) return NotFound();
            _context.Settings.Remove(delete);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int id)
        {
            if (id <= 0) return NotFound();
            Setting? setting = _context.Settings.FirstOrDefault(s => s.Id == id);
            if (setting is null) return NotFound();
            return View(setting);
        }
    }
}
