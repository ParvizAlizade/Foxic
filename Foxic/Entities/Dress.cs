using Foxic.ViewModels.Basket;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.ComponentModel.DataAnnotations.Schema;

namespace Foxic.Entities
{
	public class Dress:BaseEntity
	{
        public string Name { get; set; }
        public double Price { get; set; }
        public double Discount { get; set; }
        public double DisountPrice { get; set; }
        public string Description { get; set; }
        public string ShortDesc { get; set; }
        public string SKU { get; set; }
        public bool IsAvailable { get; set; }
        public int Availability { get; set; }
        public int Barcode { get; set; }
        public int CollectionId { get; set; }
        public Collection Collection { get; set; }
        public int GlobalTabId { get; set; }
        public GlobalTab GlobalTab { get; set; }
        public int IntroductionId { get; set; }
        public Introduction Introduction { get; set; }
        public List<DressImage> DressImages { get; set; }
        public ICollection<DressColorSize> DressColorSizes { get; set; }
        
        public List<DressCategory> DressCategories { get; set; }
        public ICollection<DressTag> DressTags { get; set; }
        [NotMapped]
        public AddCartVM AddCart { get; set; }

        public Dress()
        {
            DressImages = new List<DressImage>();
            DressColorSizes = new List<DressColorSize>();
            DressCategories = new List<DressCategory>();
            DressTags = new List<DressTag>();
            DressColorSizes=new List<DressColorSize>();
        }
    }
}
