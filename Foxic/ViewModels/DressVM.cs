using Foxic.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Foxic.ViewModels
{
    public class DressVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public double Discount { get; set; }
        public double DisountPrice { get; set; }
        public string? Description { get; set; }
        public string ?ShortDesc { get; set; }
        public string ?SKU { get; set; }
        public bool IsAvailable { get; set; }
        public int Availability { get; set; }
        public int Barcode { get; set; }

        public int CollectionId { get; set; }
        public int GlobalTabId { get; set; }

        public int IntroductionId { get; set; }

        [NotMapped]
        public ICollection<int> CategoryIds { get; set; } = null!;
        [NotMapped]
        public ICollection<int> TagIds { get; set; } = null!;
        [NotMapped]
        public IFormFile? MainPhoto { get; set; } = null!;
        [NotMapped]
        public ICollection<IFormFile>? Images { get; set; }
        [NotMapped]
        public ICollection<DressImage>? SpecificImages { get; set; }
        [NotMapped]
        public ICollection<int>? ImageIds { get; set; }
        public string? ColorSizeQuantity { get; set; }


    }
}
