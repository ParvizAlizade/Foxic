namespace Foxic.Entities
{
	public class DressCategory:BaseEntity
	{
        public Dress Dress { get; set; }
        public Category Category { get; set; }
    }
}
