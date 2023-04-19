using Foxic.DAL;
using Foxic.Entities;
using Microsoft.CodeAnalysis.Differencing;
using Foxic.Utilities.Extensions;
using Foxic.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace Foxic.Areas.FoxicAdmin.Controllers
{
    [Area("FoxicAdmin")]
	[Authorize(Roles = "Admin, Moderator")]
	public class DressController : Controller
    {

		private readonly FoxicDbContext _context;
		private readonly IWebHostEnvironment _env;

        public DressController(FoxicDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }


        public IActionResult Index(int page=1)
		{
			ViewBag.TotalPage = Math.Ceiling((double)_context.Dresses.Count() / 4);
			ViewBag.CurrentPage = page;
            ViewBag.Dresses = _context.Dresses
                                 .Include(p => p.DressImages).AsNoTracking().Skip((page - 1) * 4).Take(4).AsEnumerable();
            IEnumerable<Dress> model = _context.Dresses.Include(d => d.DressImages)
														.Include(d => d.DressColorSizes).ThenInclude(d => d.Size)
														.Include(d => d.DressColorSizes).ThenInclude(d => d.Color)
														 .AsNoTracking().Skip((page - 1) * 4).Take(4).AsEnumerable();
            return View(model);
		}




        public IActionResult Create()
        {
            ViewBag.Collections = _context.Collections.AsEnumerable();
            ViewBag.Categories = _context.Categories.AsEnumerable();
            ViewBag.Tags = _context.Tags.AsEnumerable();
            ViewBag.GlobalTabs = _context.GlobalTabs.AsEnumerable();
            ViewBag.Introductions = _context.Introductions.AsEnumerable();
            ViewBag.Sizes = _context.Sizes.AsEnumerable();
            ViewBag.Colors = _context.Colors.AsEnumerable();
            return View();
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DressVM newDress)
        {
            ViewBag.Collections = _context.Collections.AsEnumerable();
            ViewBag.Categories = _context.Categories.AsEnumerable();
            ViewBag.Tags = _context.Tags.AsEnumerable();
            ViewBag.GlobalTabs = _context.GlobalTabs.AsEnumerable();
            ViewBag.Introductions = _context.Introductions.AsEnumerable();
            ViewBag.Sizes= _context.Sizes.AsEnumerable();
            ViewBag.Colors= _context.Colors.AsEnumerable();
            TempData["InvalidImages"] = string.Empty;
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (!newDress.MainPhoto.IsValidFile("image/"))
            {
                ModelState.AddModelError(string.Empty, "Please choose image file");
                return View();
            }
            if (!newDress.MainPhoto.IsValidLength(1))
            {
                ModelState.AddModelError(string.Empty, "Please choose size must be maximum 1MB");
                return View();
            }

            Dress dress = new()
            {
                Name = newDress.Name,
                Description = newDress.Description,
                Price = newDress.Price,
                Discount=newDress.Discount,
                DisountPrice=newDress.DisountPrice,
                ShortDesc=newDress.ShortDesc,
                Barcode=newDress.Barcode,
                SKU = newDress.SKU,
                IsAvailable=newDress.IsAvailable,
                Availability=newDress.Availability,
                CollectionId = newDress.CollectionId,
                GlobalTabId=newDress.GlobalTabId,
                IntroductionId=newDress.IntroductionId
            };
            string imageFolderPath = Path.Combine(_env.WebRootPath, "assets", "images");
            foreach (var image in newDress.Images)
            {
                if (!image.IsValidFile("image/") || !image.IsValidLength(1))
                {
                    TempData["InvalidImages"] += image.FileName;
                    continue;
                }
                DressImage dressImage = new()
                {
                    IsMain = false,
                    Path = await image.CreateImage(imageFolderPath, "products")
                };
                dress.DressImages.Add(dressImage);
            }
            string[] colorSizeQuantities = newDress.ColorSizeQuantity.Split(',');
            foreach (string colorSizeQuantity in colorSizeQuantities)
            {
                string[] datas = colorSizeQuantity.Split('-');
                DressColorSize dressColorSize = new()
                {
                    SizeId = int.Parse(datas[0]),
                    ColorId = int.Parse(datas[1]),
                    Quantity = int.Parse(datas[2])
                };
                dress.DressColorSizes.Add(dressColorSize);
            }
            DressImage main = new()
            {
                IsMain = true,
                Path = await newDress.MainPhoto.CreateImage(imageFolderPath, "products")
            };
            dress.DressImages.Add(main);
           

            foreach (int id in newDress.CategoryIds)
            {
                DressCategory category = new()
                {
                    CategoryId = id,
                };
                dress.DressCategories.Add(category);
            }
            foreach (int id in newDress.TagIds)
            {
                DressTag tag = new()
                {
                    TagId = id
                };
                dress.DressTags.Add(tag);
            }
            _context.Dresses.Add(dress);
            _context.SaveChanges();
            return RedirectToAction("Index", "Dress");
        }





        public IActionResult Details(int id)
        {
            if (id <= 0) return BadRequest();
            Dress? dress = _context.Dresses
                .Include(d => d.DressImages)
                .Include(d => d.Collection)
                .Include(d=>d.DressCategories)
                .Include(d=>d.DressTags)
                .Include(d=>d.Introduction)
                .Include(d => d.GlobalTab).FirstOrDefault(d=>d.Id==id);
            if (dress is null) return BadRequest();
            return View(dress);
        }




        public IActionResult Delete(int id)
        {
            if (id <= 0) return NotFound();
            Dress dress=_context.Dresses.FirstOrDefault(d=> d.Id==id);  
            if (dress is null) return BadRequest();
            return View(dress);
        }




        [HttpPost]
        public async Task<IActionResult> Delete(int id,Dress deleted)
        {
            if(id<=0) return BadRequest();
            Dress? dress = await _context.Dresses.Include(d => d.DressCategories)
                .Include(d => d.DressImages)
                .Include(d => d.Collection)
                .Include(d => d.DressCategories)
                .Include(d => d.DressTags)
                .Include(d => d.Introduction)
                .Include(d => d.GlobalTab).FirstOrDefaultAsync(d => d.Id == id);
            if (dress is null) return NotFound();
            if (id == deleted.Id)
            {
                _context.Dresses.Remove(dress);
                _context.SaveChanges();
            }
            return RedirectToAction("Index","Dress");
        }
    }
}
