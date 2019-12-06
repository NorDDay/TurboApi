using Microsoft.EntityFrameworkCore;
using TurboApi.Models;

namespace TurboApi.Services
{
	public class DbLocalContext : DbContext
	{
		public DbSet<User> Users { get; set; }
		public DbLocalContext(DbContextOptions<DbLocalContext> options)
			: base(options)
		{
			Database.EnsureCreated();
		}
	}
}
