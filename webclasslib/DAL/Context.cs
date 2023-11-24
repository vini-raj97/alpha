using Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
	public class Context : DbContext
	{
		public Context(DbContextOptions<Context> options)
			: base(options) { }

		public DbSet<Info>? ClientInfo { get; set; }
		public DbSet<TeamInfo>? TeamInfo { get; set; }
		//public DbSet<BuildVersions>? BuildVersions { get; set; }

		// Add other DbSet properties as needed

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			// Configure your model here if needed
		}
	}
}
