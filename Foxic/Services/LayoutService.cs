using Foxic.DAL;
using Foxic.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Foxic.Services
{
	public class LayoutService
	{
		public FoxicDbContext _context { get; } 
        public LayoutService(FoxicDbContext context)
        {
			_context= context;
		}


		public List<Setting> GetSettings()
		{
			List<Setting >settings = _context.Settings.ToList();
			return settings;
		}
	}
}
