using Microsoft.AspNetCore.Mvc;

namespace Foxic.Areas.FoxicAdmin.Controllers
{
    public class SettingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }
     
    }
}
