using Foxic.Entities;
using Microsoft.EntityFrameworkCore;

namespace Foxic.DAL
{
    public class FoxicDbContext :DbContext
    {
        public FoxicDbContext(DbContextOptions<FoxicDbContext>options):base(options) 
        {

        }

		public DbSet<Setting> Settings { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<Collection> Collections { get; set; }
		public DbSet<Color> Colors { get; set; }
		public DbSet<Dress> Dresses { get; set; }
		public DbSet<DressCategory> DressCategories { get; set; }
		public DbSet<DressColorSize> DressColorSizes { get; set; }
		public DbSet<DressImage> DressImages { get; set; }
		public DbSet<DressTag> DressTags { get; set; }
		public DbSet<GlobalTab> GlobalTabs { get; set; }
		public DbSet<Introduction> Introductions { get; set; }
		public DbSet<Tag> Tags { get; set; }
		public DbSet<Slider> Sliders { get; set; }
		public DbSet<Size> Sizes { get; set; }





		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Setting>()
				.HasIndex(s => s.Key)
				.IsUnique();
		}
	}
}
