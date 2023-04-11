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


		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Setting>()
				.HasIndex(s => s.Key)
				.IsUnique();
		}
	}
}
