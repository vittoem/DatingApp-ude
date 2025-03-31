namespace API.Entities;

// Class will represent a table in the DB
public class AppUser
{
	// EF requires public properties
	// Id will be the primary key in the DB - EF will increment it automatically.
	public int Id { get; set; }

	public required string UserName { get; set; }
}
