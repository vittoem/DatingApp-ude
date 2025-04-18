using DatingApp.API.Extensions;

namespace API.Entities;

// Class will represent a table in the DB
public class AppUser
{
	// EF requires public properties
	// Id will be the primary key in the DB - EF will increment it automatically.
	public int Id { get; set; }

	public required string UserName { get; set; }

	public byte[] PasswordHash { get; set; } = [];

	/// <summary>
	/// Gets or sets the cryptographic salt used in conjunction with the password hash to enhance security.
	/// </summary>
	public byte[] PasswordSalt { get; set; } = [];

	public DateOnly DateOfBirth { get; set; }

	public required string KnownAs { get; set; }

	public DateTime Created { get; set; } = DateTime.UtcNow;

	public DateTime LastActive { get; set; } = DateTime.UtcNow;

	public required string Gender { get; set; }

	public string? Introduction { get; set; }
	public string? LookingFor { get; set; }
	public string? Interests { get; set; }
	public required string City { get; set; }
	public required string Country { get; set; }

	public IList<Photo> Photos { get; set; } = [];

	/*public int GetAge() {
		return DateOfBirth.CalculateAge();
	} */
}
