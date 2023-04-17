using Foxic.DAL;
using Foxic.Entities;
using Foxic.ViewModels.Cookie;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;

namespace Foxic.Services
{
	public class LayoutService
	{
        private readonly FoxicDbContext _context;
        private readonly IHttpContextAccessor _accessor;

        public LayoutService(FoxicDbContext context, IHttpContextAccessor accessor)
        {
            _context = context;
            _accessor = accessor;
        }


        public List<Setting> GetSettings()
		{
			List<Setting >settings = _context.Settings.ToList();
			return settings;
		}

        public CookieBasketVM GetBasket()
        {
            var cookies = _accessor.HttpContext.Request.Cookies["basket"];
            CookieBasketVM basket;
            if (cookies is not null)
            {
                basket = JsonConvert.DeserializeObject<CookieBasketVM>(cookies);
                foreach (CookieBasketItemVM item in basket.CookieBasketItemVMs)
                {
                    Dress dress = _context.Dresses.FirstOrDefault(d => d.Id == item.Id);
                    if (dress is null)
                    {
                        basket.CookieBasketItemVMs.Remove(item);
                        basket.TotalPrice -= item.Quantity * item.Price;
                    }
                }
                return basket;
            }
            return null;
        }
    }
}
