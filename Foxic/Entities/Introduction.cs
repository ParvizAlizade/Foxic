namespace Foxic.Entities
{
	public class Introduction :BaseEntity
	{
        public string Polyester { get; set; }
        public string Lining { get; set; }

        public string DryClean { get; set; }
        public string Chlorine { get; set; }
        public List<Dress> Dresses{ get; set; }
    }
}
