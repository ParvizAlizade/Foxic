namespace Foxic.Entities
{
	public class DressImage:BaseEntity
	{
		public string Path { get; set; }
		public bool? IsMain { get; set; }
		public Dress Dress { get; set; }
    }
}
