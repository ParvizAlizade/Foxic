namespace Foxic.Entities
{
	public class DressTag:BaseEntity
	{
        public Tag Tag { get; set; }
        public Dress Dress { get; set; }
    }
}
