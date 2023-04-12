namespace Foxic.Entities
{
	public class Category:BaseEntity
	{
        public string Name { get; set; }
        public List<DressCategory> DressCategories { get; set; }
		public Category()
		{
			DressCategories = new();
		}
	}
}
