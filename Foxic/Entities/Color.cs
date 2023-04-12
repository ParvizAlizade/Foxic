namespace Foxic.Entities
{
	public class Color :BaseEntity
	{
        public string Name { get; set; }
        public ICollection<DressColorSize> DressColorSizes { get; set; }
    }
}
