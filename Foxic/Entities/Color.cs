using System.ComponentModel.DataAnnotations.Schema;

namespace Foxic.Entities
{
	public class Color :BaseEntity
	{
        public string Name { get; set; }
        public string ColorPath { get; set; }
        [NotMapped]
        public List<DressColorSize> DressColorSizes { get; set; }

        [NotMapped]
        public IFormFile? Image { get; set; }
        public Color()
        {
            DressColorSizes = new();
        }
    }
}
