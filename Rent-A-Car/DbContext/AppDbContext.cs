using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Rent_A_Car.Models;

namespace Rent_A_Car.DbContext
{
	public class AppDbContext : IdentityDbContext<User>
	{
		public AppDbContext(DbContextOptions options) : base(options)
		{
		}
		public DbSet<User> Users { get; set; }
		public DbSet<Car> Car { get; set; }
		public DbSet<Request> Request { get; set; }
	}
}
