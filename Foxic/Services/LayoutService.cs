using Foxic.DAL;
using Foxic.Entities;
using Foxic.ViewModels.Basket;
using Foxic.ViewModels.Cookie;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;

namespace Foxic.Services
{
    public class LayoutService
    {
        private readonly FoxicDbContext _context;
        private readonly IHttpContextAccessor _accessor;
        private readonly UserManager<User> _userManager;

        public LayoutService(FoxicDbContext context, IHttpContextAccessor accessor, UserManager<User> userManager)
        {
            _context = context;
            _accessor = accessor;
            _userManager = userManager;
        }


        public List<Setting> GetSettings()
        {
            List<Setting> settings = _context.Settings.ToList();
            return settings;
        }

        public List<BasketItem>? GetBasketItems()
        {
            List<BasketItem> basket = _context.BasketItems.Include(b => b.Dresscolorsize.Dress).ThenInclude(dcs => dcs.DressImages).ToList();
            return basket;
        }

        public List<Dress> GetDresses()
        {
            List<Dress> dresses = _context.Dresses.Include(d => d.DressImages).ToList();
            return dresses;
        }

        public List<BasketItemVM> GetBasketItem()
        {
            List<BasketItemVM> items = new();
            User useer = new();

            if (_accessor.HttpContext.User.Identity.IsAuthenticated)
            {
                useer = _userManager.Users.FirstOrDefault(u=>u.UserName==_accessor.HttpContext.User.Identity.Name);

            }
            List<BasketItem> basketItems=_context.BasketItems.Include(bi=>bi.Dresscolorsize.Dress)
                                    .ThenInclude(d=>d.DressImages)
                                        .Where(a=>a.Basket.User.Id==useer.Id)
                                            .ToList();
            items = basketItems.Select(b => new BasketItemVM
            {
                ProductId=b.Dresscolorsize.DressId,
                Quantity=b.SaleQuantity,
                ProductSizeColorId=b.ProductSizeColorId,
                Price=(decimal)b.Dresscolorsize.Dress.DisountPrice
            }).ToList();
            return items;
        }


    }
}
