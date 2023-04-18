using NuGet.ContentModel;

namespace Foxic.Entities
{
    public class BasketItem:BaseEntity
    {
        public int SaleQuantity { get; set; }

        public Basket Basket { get; set; }
        public DressColorSize Dresscolorsize { get; set; }
        public int BasketId { get; set; }
        public decimal UnitPrice { get; set; }

        public int ProductSizeColorId { get; set; }
    }
}
