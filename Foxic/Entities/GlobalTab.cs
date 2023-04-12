namespace Foxic.Entities
{
	public class GlobalTab:BaseEntity
	{
        public string Description { get; set; }
        public ICollection<Dress> Dresses { get; set; }
    }
}
