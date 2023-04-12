namespace Foxic.Entities
{
	public class DressTag:BaseEntity
	{
        public int TagId { get; set; }
        public Tag Tag { get; set; }
        public Dress Dress { get; set; }
    }
}
