namespace Foxic.ViewModels.Basket
{
    public class BasketVM
    {
        public List<BasketItemVM> BasketItems { get; set; }
        public decimal TotalPrice { get; set; }

        public BasketVM()
        {
            BasketItems = new();
        }
    }
}
