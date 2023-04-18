using Foxic.DAL;
using Foxic.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Foxic.Controllers
{
    public class ContactUsController : Controller
    {
        private readonly FoxicDbContext _context;
        public ContactUsController(FoxicDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Setting> settings = _context.Settings.ToList();
            return View(settings);
        }
    }
}
