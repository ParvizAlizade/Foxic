using Microsoft.AspNetCore.Mvc;

namespace Foxic.Entities
{
	public class Tag:BaseEntity
	{
		public string Name { get; set; }
		public List<DressTag> DressTags { get; set; }
		public Tag()
		{
			DressTags = new();
		}
	}
}
