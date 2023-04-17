namespace Foxic.ViewModels.Cookie
{
    public class CookieBasketVM
    {
        public List<CookieBasketItemVM> CookieBasketItemVMs { get; set; }
        public double TotalPrice { get; set; }

        public CookieBasketVM()
        {
            CookieBasketItemVMs = new ();
        }

    }
}
