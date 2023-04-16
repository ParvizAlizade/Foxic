using Foxic.DAL;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

namespace Foxic.Entities
{
	public class Slider :  BaseEntity
	{
        public string? ImagePath { get; set; }
        public string Desc { get; set; }
        public string Link { get; set; }
        public string AdvName { get; set; }
        public int Order { get; set; }
        public string Button { get; set; }
        [NotMapped]
		public IFormFile? Image { get; set; }
	}
}
