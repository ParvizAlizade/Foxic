namespace Foxic.Entities
{
	public class DressColorSize :BaseEntity
	{
        public int DressId { get; set; }
        public int ColorId { get; set; }
        public int SizeId { get; set; }
        public int Quantity { get; set; }
        public Dress Dress { get; set; }
        public Color Color { get; set; }

        public Size Size { get; set; }
    }
}
