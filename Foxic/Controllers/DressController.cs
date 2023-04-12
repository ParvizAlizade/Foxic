using Foxic.DAL;
using Foxic.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace Foxic.Controllers
{
	public class DressController : Controller
	{
		readonly FoxicDbContext _context;
		public DressController(FoxicDbContext context)
		{
			_context = context;
		}
		public IActionResult Index()
		{
			List<Dress> dresses=_context.Dresses.Include(d=>d.DressTags).ToList();

			return View(dresses);
		}
		public IActionResult Details()
		{
			return View();
		}

	}
}
