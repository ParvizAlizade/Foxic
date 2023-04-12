namespace Foxic.Entities
{
	public class Collection:BaseEntity
	{
        public string Name { get; set; }
		public List<Dress>Dresses { get; set; }
        
    }
}
