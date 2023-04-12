namespace Foxic.Entities
{
	public class Size:BaseEntity
	{
        public string Name { get; set; }
        public ICollection<DressColorSize> DressColorSizes { get; set; }
    }
}
