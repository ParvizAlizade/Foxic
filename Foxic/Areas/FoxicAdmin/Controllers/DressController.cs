using Foxic.DAL;
using Foxic.Entities;
using Microsoft.CodeAnalysis.Differencing;
using Foxic.Utilities.Extensions;
using Foxic.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Xml.Linq;


namespace Foxic.Areas.FoxicAdmin.Controllers
{
    [Area("FoxicAdmin")]
    public class DressController : Controller
    {

		private readonly FoxicDbContext _context;
		private readonly IWebHostEnvironment _env;

		public DressController(FoxicDbContext context, IWebHostEnvironment env)
		{
			_context = context;
			_env = env;
		}
		public IActionResult Index()
		{
			IEnumerable<Dress> model = _context.Dresses.Include(d => d.DressImages)
														.Include(d => d.DressColorSizes).ThenInclude(d => d.Size)
														.Include(d => d.DressColorSizes).ThenInclude(d => d.Color)
														 .AsNoTracking().AsEnumerable();
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
                DressImage plantImage = new()
                {
                    IsMain = false,
                    Path = await image.CreateImage(imageFolderPath, "products")
                };
                dress.DressImages.Add(plantImage);
            }
            string[] colorSizeQuantities = newDress.ColorSizeQuantity.Split(',');
            foreach (string colorSizeQuantity in colorSizeQuantities)
            {
                string[] datas = colorSizeQuantity.Split('-');
                DressColorSize plantSizeColor = new()
                {
                    SizeId = int.Parse(datas[0]),
                    ColorId = int.Parse(datas[1]),
                    Quantity = int.Parse(datas[2])
                };
                dress.DressColorSizes.Add(plantSizeColor);
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

        public IActionResult Edit(int id)
        {
            if (id == 0) return BadRequest();
            DressVM? model = EditedDress(id);

            ViewBag.Collections = _context.Collections.AsEnumerable();
            ViewBag.Categories = _context.Categories.AsEnumerable();
            ViewBag.Tags = _context.Tags.AsEnumerable();
            ViewBag.GlobalTabs = _context.GlobalTabs.AsEnumerable();
            ViewBag.Introductions = _context.Introductions.AsEnumerable();
            ViewBag.Sizes = _context.Sizes.AsEnumerable();
            ViewBag.Colors = _context.Colors.AsEnumerable();
            if (model is null) return BadRequest();
            _context.SaveChanges();
            return View(model);
        }


        private DressVM? EditedDress(int id)
        {
            DressVM? model = _context.Dresses.Include(d => d.DressCategories)
                                            .Include(d => d.DressTags)
                                            .Include(d => d.DressImages)
                                            .Include(d=>d.Collection)
                                            .Include(d=>d.Introduction)
                                            .Include(d=>d.GlobalTab)
                                            

                                            .Select(p =>
                                                new DressVM
                                                {
                                                    Id = p.Id,
                                                    Name = p.Name,
                                                    SKU = p.SKU,
                                                    Description = p.Description,
                                                    Price = p.Price,
                                                    ShortDesc= p.ShortDesc,
                                                    IsAvailable=p.IsAvailable,
                                                    Availability = p.Availability,  
                                                    Barcode= p.Barcode,
                                                    CollectionId= p.CollectionId,
                                                    GlobalTabId= p.GlobalTabId,
                                                    IntroductionId= p.IntroductionId,
                                                    DisountPrice = p.DisountPrice,
                                                    CategoryIds = p.DressCategories.Select(pc => pc.CategoryId).ToList(),
                                                    TagIds = p.DressTags.Select(pc => pc.TagId).ToList(),
                                                    SpecificImages = p.DressImages.Select(p => new DressImage
                                                    {
                                                        Id = p.Id,
                                                        Path = p.Path,
                                                        IsMain = p.IsMain
                                                    }).ToList()
                                                })
                                                .FirstOrDefault(p => p.Id == id);
            return model;
        }




        public async Task<IActionResult> Edit(int id, DressVM edited)
        {
            ViewBag.Categories = _context.Categories.AsEnumerable();
            ViewBag.Tags = _context.Tags.AsEnumerable();
            DressVM? model = EditedDress(id);

            Dress? dress = await _context.Dresses.Include(d => d.DressImages).FirstOrDefaultAsync(d => d.Id == id);
            if (dress is null) return BadRequest();

            IEnumerable<string> removables = dress.DressImages.Where(p => !edited.ImageIds.Contains(p.Id)).Select(i => i.Path).AsEnumerable();
            string imageFolderPath = Path.Combine(_env.WebRootPath, "assets", "images");
            foreach (string removable in removables)
            {
                string path = Path.Combine(imageFolderPath, "products", removable);
                await Console.Out.WriteLineAsync(path);
                Console.WriteLine(FileUpload.DeleteImage(path));
            }

            if (edited.MainPhoto is not null)
            {
                await AdjustDressPhotos(edited.MainPhoto, dress, true);
            }
           
            dress.DressImages.RemoveAll(p => !edited.ImageIds.Contains(p.Id));
            if (edited.Images is not null)
            {
                foreach (var item in edited.Images)
                {
                    if (!item.IsValidFile("image/") || !item.IsValidLength(1))
                    {
                        TempData["InvalidImages"] += item.FileName;
                        continue;
                    }
                    DressImage dressImage = new()
                    {
                        IsMain = false,
                        Path = await item.CreateImage(imageFolderPath, "website-images")
                    };
                    dress.DressImages.Add(dressImage);
                }
            }
            dress.Name = edited.Name; 
            dress.Price = edited.Price;
            dress.Description = edited.Description; 
            dress.SKU=edited.SKU; 
            dress.Discount= edited.Discount;  
            dress.Barcode  = edited.Barcode;  
            dress.Availability = edited.Availability; 
            dress.IsAvailable = edited.IsAvailable;
            dress.ShortDesc= edited.ShortDesc;   
            dress.CollectionId = edited.CollectionId;
            dress.IntroductionId = edited.IntroductionId;  
            dress.GlobalTabId = edited.GlobalTabId;  
            dress.DisountPrice = edited.DisountPrice;
            
            _context.SaveChanges();
            //TODO Edit Category and Tag IDs
            return Json(dress.DressImages.Select(p => p.Path));
        }





        private async Task AdjustDressPhotos(IFormFile image, Dress dress, bool? isMain)
        {
            string photoPath = dress.DressImages.FirstOrDefault(d => d.IsMain == isMain).Path;
            string imagesFolderPath = Path.Combine(_env.WebRootPath, "assets", "images");
            string filePath = Path.Combine(imagesFolderPath, "products", photoPath);
            FileUpload.DeleteImage(filePath);
            dress.DressImages.FirstOrDefault(d => d.IsMain == isMain).Path = await image.CreateImage(imagesFolderPath, "products");
        }


        public IActionResult Search(string data)
        {
            List<Dress> dress = _context.Dresses.Where(d => d.Name.Contains(data)).ToList();
            return Json(dress);
        }


    }
}
