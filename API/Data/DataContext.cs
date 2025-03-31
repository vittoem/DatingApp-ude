using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

// Primary constructor for DbContext
// This class is responsible for interacting with the database.
public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)	
{

	// Add DbSet properties for your entities here
	// Users will ne name of the table in the DB
	public DbSet<AppUser> Users { get; set; }
}
