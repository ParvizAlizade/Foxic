using Foxic.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Foxic.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

    }
}