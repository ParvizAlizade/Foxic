namespace Foxic.Entities
{
	public class Size:BaseEntity
	{
        public string Name { get; set; }
        public List<DressColorSize> DressColorSizes { get; set; }
		public Size()
		{
			DressColorSizes = new();
		}
	}
}
