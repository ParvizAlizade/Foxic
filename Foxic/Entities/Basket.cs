namespace Foxic.Entities
{
    public class Basket:BaseEntity
    {
        public User User { get; set; }
        public ICollection<BasketItem> BasketItems { get; set; } = null!;
        public decimal TotalPrice { get; set; }
        public Order Order { get; set; }
        public bool IsOrdered { get; set; } = false;

        public Basket()
        {
            BasketItems = new List<BasketItem>();
        }
    }
}
