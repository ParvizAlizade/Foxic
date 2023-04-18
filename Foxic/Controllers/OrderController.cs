using Foxic.DAL;
using Foxic.Entities;
using Foxic.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Foxic.Controllers
{
	public class OrderController : Controller
	{
        private readonly UserManager<User> _userManager;

        readonly FoxicDbContext _context;
        public OrderController(FoxicDbContext context,UserManager<User>userManager)
        {
            _context = context;
            _userManager = userManager;
        }

		public IActionResult Index()
		{
            if (!User.Identity.IsAuthenticated)
            {
                return View(new List<WishListItemVM>());
            }

            var userId = _userManager.GetUserId(User);

            var wishListItems = _context.WishListItems.Include(wli=>wli.Dress)
                                    .ThenInclude(wlid=>wlid.DressImages)
                                         .Where(wli=>wli.UserId==userId).ToList();

            if(wishListItems.Count==0) 
            {
                return View(new List<WishListItem>());
            }
            return View(wishListItems);

        }

        public async Task<IActionResult> AddToWishList(int dressIdd)
        {
            Dress dress=await _context.Dresses.FindAsync(dressIdd);
            if(dress==null) { return NotFound(); }

            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login","Account");
            }
            User user=await _userManager.FindByNameAsync(User.Identity.Name);
            WishListItem userWishListItem=await _context.WishListItems.FirstOrDefaultAsync(w=>w.UserId==user.Id && w.DressId==dressIdd);

            if(userWishListItem==null)
            {
                userWishListItem = new WishListItem
                {
                    UserId = user.Id,
                    DressId = dressIdd,
                };
                _context.WishListItems.Add(userWishListItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult>  RemoveFromWishList  (int wishlistitemid)
        {
            User user = await _userManager.FindByNameAsync(User.Identity.Name);
            WishListItem? wishListItem = await _context.WishListItems
                            .FirstOrDefaultAsync(wli => wli.UserId == user.Id && wli.Id == wishlistitemid);

            if (wishListItem==null) 
            {
                return NotFound();
            }
            _context.WishListItems.Remove(wishListItem);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Addbaskets(int dressid, Dress basketdress)
        {
            DressColorSize? dressColorSize = _context.DressColorSizes
                .Include(d => d.Dress)
                .FirstOrDefault(d => d.DressId == dressid && d.SizeId == basketdress.AddCart.SizeId && d.ColorId == basketdress.AddCart.ColorId);

            if (dressColorSize is null) return NotFound();
            User? user = new();

            if (User.Identity.IsAuthenticated)
            {
                user = await _userManager.FindByNameAsync(User.Identity.Name);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            BasketItem? item = _context.BasketItems.FirstOrDefault(b => b.Dresscolorsize == dressColorSize);
            Basket? userActiveBasket = _context.Baskets
                .Include(b => b.User)
                .Include(b => b.BasketItems)
                .ThenInclude(bi => bi.Dresscolorsize)
                .FirstOrDefault(b => b.User.Id == user.Id && !b.IsOrdered);

            if (userActiveBasket is null)
            {
                userActiveBasket = new Basket()
                {
                    User = user,
                    BasketItems = new List<BasketItem>(),
                };
                _context.Baskets.Add(userActiveBasket);
            }

            BasketItem? items = userActiveBasket.BasketItems.FirstOrDefault(b => b.Dresscolorsize == dressColorSize);

            if (items is not null)
            {
                items.SaleQuantity += basketdress.AddCart.Quantity;
            }
            else
            {
                items = new BasketItem
                {
                    Dresscolorsize = dressColorSize,
                    SaleQuantity = basketdress.AddCart.Quantity,
                    UnitPrice = (decimal)dressColorSize.Dress.DisountPrice,
                    Basket = userActiveBasket
                };
                userActiveBasket.BasketItems.Add(items);
            }
            userActiveBasket.TotalPrice = userActiveBasket.BasketItems.Sum(p => p.SaleQuantity * p.UnitPrice);
            await _context.SaveChangesAsync();
            return RedirectToAction("index", "home");
        }

        public async Task<IActionResult> RemoveBasketItem(int basketItemId)
        {
            User? user = null; 
            
            if (User.Identity.IsAuthenticated)
            {
                user = await _userManager.FindByNameAsync(User.Identity.Name);
            }
            BasketItem? item = _context.BasketItems.FirstOrDefault(i => i.Id == basketItemId);

            if (item is not null)
            {
                Basket? userActiveBasket = _context.Baskets
                    .Include(b => b.User)
                    .Include(b => b.BasketItems)
                    .ThenInclude(i => i.Dresscolorsize)
                    .FirstOrDefault(b => b.User.Id == user.Id && !b.IsOrdered);
                if (userActiveBasket is not null)
                {
                    userActiveBasket.BasketItems.Remove(item);
                    userActiveBasket.TotalPrice = userActiveBasket.BasketItems.Sum(p => p.SaleQuantity * p.UnitPrice);

                    await _context.SaveChangesAsync();
                }
            }
            return RedirectToAction("index", "home");
        }

    }
}
