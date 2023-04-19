using Foxic.DAL;
using Foxic.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography.X509Certificates;

namespace Foxic.Controllers
{
    public class DressComparer : IEqualityComparer<Dress>
    {
        public bool Equals(Dress? x, Dress? y)
        {
            if (Equals(x?.Id, y?.Id)) return true;
            return false;
        }

        public int GetHashCode([DisallowNull] Dress obj)
        {
            throw new NotImplementedException();
        }
    }

    public class DressCategoryComparer : IEqualityComparer<DressCategory>
    {
        public bool Equals(DressCategory? x, DressCategory? y)
        {
            if (Equals(x?.Category.Id, y?.Category.Id)) return true;
            return false;
        }

        public int GetHashCode([DisallowNull] DressCategory obj)
        {
            throw new NotImplementedException();
        }
    }





    public class DressController : Controller
	{
		readonly FoxicDbContext _context;
		public DressController(FoxicDbContext context)
		{
			_context = context;
		}
		public IActionResult Index()
		{
			List<Dress> dresses=_context.Dresses
									.Include(d=>d.Collection)
										.Include(d=>d.DressImages).ToList();
			ViewBag.Dresses = _context.Dresses
									 .Include(p => p.DressImages).ToList();
			return View(dresses);
		}


		public IActionResult Details(int id)
		{
			if (id == 0) return NotFound();
			IQueryable<Dress> dresses = _context.Dresses.AsNoTracking().AsQueryable();
            Dress? dress = dresses
                                .Include(d => d.DressImages)
                                        .Include(d => d.DressTags)
                                             .ThenInclude(dt => dt.Tag)
                                              .Include(d => d.Collection)
                                              .Include(d => d.Introduction)
                                               .Include(d => d.DressColorSizes).ThenInclude(dsc => dsc.Color)
                                               .Include(dsc => dsc.DressColorSizes).ThenInclude(a=>a.Size)
												.Include(d => d.DressCategories)
												 .ThenInclude(dc => dc.Category).AsSingleQuery().FirstOrDefault(d => d.Id == id);

			if (dress is null) return NotFound();


			ViewBag.Colors = dress.DressColorSizes.DistinctBy(p => p.ColorId).Select(p => new Color() { Id = p.ColorId, Name = p.Color.Name }).ToList();
			ViewBag.Sizes = dress.DressColorSizes.DistinctBy(p => p.SizeId).Select(p => new Size() { Id = p.SizeId, Name = p.Size.Name }).ToList();

			ViewBag.MainPath = _context.DressImages.FirstOrDefault(i=> (bool)i.IsMain);
			ViewBag.Relateds = RelatedDresses(dresses, dress, id);
			return View(dress);
		}


        static List<Dress> RelatedDresses(IQueryable<Dress> queryable, Dress dress, int id)
        {
            List<Dress> relateds = new();

            dress.DressCategories.ForEach(pc =>
            {
                List<Dress> related = queryable
                    .Include(p => p.DressImages)
                    .Include(d => d.Collection)
                        .Include(p => p.DressCategories)
                            .ThenInclude(pc => pc.Category)
                                    //.AsSingleQuery()
                                    .AsEnumerable()
                                        .Where(
                                        p => p.DressCategories.Contains(pc, new DressCategoryComparer())
                                        && p.Id != id
                                        && !relateds.Contains(p, new DressComparer())
                                        )
                                        .ToList();
                relateds.AddRange(related);
            });
            return relateds;
        }

    }
}
