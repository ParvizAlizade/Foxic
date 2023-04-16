using Foxic.DAL;
using Foxic.Entities;
using Foxic.Utilities.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Foxic.Areas.FoxicAdmin.Controllers
{
	[Area("FoxicAdmin")]
	//[Authorize(Roles = "Admin, Moderator")]
	public class GlobalTabController : Controller
	{
		private readonly FoxicDbContext _context;

		public GlobalTabController(FoxicDbContext context)
		{
			_context = context;
		}
		public IActionResult Index()
		{
			IEnumerable<GlobalTab> globalTabs = _context.GlobalTabs.AsEnumerable();
			return View(globalTabs);
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[AutoValidateAntiforgeryToken]
		public IActionResult Create(GlobalTab newglobaltab)
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
			bool isDuplicated = _context.GlobalTabs.Any(c => c.Description == newglobaltab.Description);
			if (isDuplicated)
			{
				ModelState.AddModelError("", "You cannot duplicate value");
				return View();
			}
			_context.GlobalTabs.Add(newglobaltab);
			_context.SaveChanges();


			return RedirectToAction(nameof(Index));
		}


		public IActionResult Edit(int id)
		{
			if (id == 0) return NotFound();
			GlobalTab globalTab = _context.GlobalTabs.FirstOrDefault(c => c.Id == id);
			if (globalTab is null) return NotFound();
			return View(globalTab);
		}

		[HttpPost]
		[AutoValidateAntiforgeryToken]
		public IActionResult Edit(int id, GlobalTab edited)
		{
			if (id != edited.Id) return BadRequest();
			GlobalTab globalTab = _context.GlobalTabs.FirstOrDefault(c => c.Id == id);
			if (globalTab is null) return NotFound();
			bool duplicate = _context.GlobalTabs.Any(c => c.Description == edited.Description && edited.Description != globalTab.Description);
			if (duplicate)
			{
				ModelState.AddModelError("", "You cannot duplicate");
				return View(globalTab);
			}
			globalTab.Description = edited.Description;
			_context.SaveChanges();
			return RedirectToAction(nameof(Index));
		}



		public IActionResult Delete(int id)
		{
			if (id == 0) return NotFound();
			GlobalTab globalTab = _context.GlobalTabs.FirstOrDefault(c => c.Id == id);
			if (globalTab is null) return NotFound();
			return View(globalTab);
		}


		[HttpPost]
		public IActionResult Delete(int id, GlobalTab delete)
		{
			if (id != delete.Id) return BadRequest();
			GlobalTab globalTab = _context.GlobalTabs.FirstOrDefault(c => c.Id == id);
			if (globalTab is null) return NotFound();
			delete = _context.GlobalTabs.FirstOrDefault(_c => _c.Id == id);
			if (delete is null) return NotFound();
			_context.GlobalTabs.Remove(delete);
			_context.SaveChanges();
			return RedirectToAction(nameof(Index));
		}

        public IActionResult Details(int id)
        {
            if (id <= 0) return NotFound();
			GlobalTab globalTab = _context.GlobalTabs.FirstOrDefault(c=>c.Id==id);
            if (globalTab is null) return NotFound();
            return View(globalTab);
        }
    }
}
