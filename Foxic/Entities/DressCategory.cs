namespace Foxic.Entities
{
	public class DressCategory:BaseEntity
	{
        public int CategoryId { get; set; }
        public Dress Dress { get; set; }
        public Category Category { get; set; }
    }
}
