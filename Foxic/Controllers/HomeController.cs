using Foxic.DAL;
using Foxic.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Foxic.Controllers
{
    public class HomeController : Controller
    {
		readonly FoxicDbContext _context;
		public HomeController(FoxicDbContext context)
		{
			_context = context;
		}
		public IActionResult Index()
		{
			List<Slider> slider = _context.Sliders.OrderBy(s => s.Order).ToList();
			ViewBag.Dresses = _context.Dresses
									 .Include(p => p.DressImages).Include(p => p.Collection)
										 .OrderByDescending(p => p.Id)
											 .Take(4)
												 .ToList();

			return View(slider);
		}

	}
}