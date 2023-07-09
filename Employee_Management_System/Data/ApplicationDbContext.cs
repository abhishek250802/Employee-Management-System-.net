using EmployeeManagement.Model;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Data
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}
		public DbSet<User> user { get; set; }
		public DbSet<Designation> designation { get; set; }
		public DbSet<Department> department { get; set; }
		public DbSet<Employees> employee { get; set; }


	}
}
