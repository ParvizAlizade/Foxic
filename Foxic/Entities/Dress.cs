using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Foxic.Entities
{
	public class Dress:BaseEntity
	{
        public string Name { get; set; }
        public int Price { get; set; }
        public int Discount { get; set; }
        public int DisountPrice { get; set; }
        public string Description { get; set; }
        public string ShortDesc { get; set; }
        public string LongDesc{ get; set; }
        public string SKU { get; set; }
        public bool IsAvailable { get; set; }
        public int Availability { get; set; }
        public int Barcode { get; set; }
        public Collection Collection { get; set; }
        public GlobalTab GlobalTab { get; set; }
        public Introduction Introduction { get; set; }
        public ICollection<DressImage> DressImages { get; set; }
        public ICollection<DressColorSize> DressColorSizes { get; set; }
        public ICollection<DressCategory> DressCategories { get; set; }
        public ICollection<DressTag> DressTags { get; set; }

        public Dress()
        {
            DressImages = new List<DressImage>();
            DressColorSizes = new List<DressColorSize>();
            DressCategories = new List<DressCategory>();
            DressTags = new List<DressTag>();
        }
    }
}
