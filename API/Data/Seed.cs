
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using API.Data;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.Data
{
	public static class Seed
	{
		public static async Task SeedUserData(DataContext context)
		{
			if (await context.Users.AnyAsync()) return;

			var userData = await File.ReadAllTextAsync("Data/UserSeedData.json");
			var options = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true
			};
			// Deserialize the JSON data into a list of AppUser objects
			var users = JsonSerializer.Deserialize<List<AppUser>>(userData, options);

			if (users == null) return;

			foreach (var user in users)
			{
				using var hmac = new HMACSHA512();
				user.UserName = user.UserName.ToLower();
				user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd"));
				user.PasswordSalt = hmac.Key;

				context.Users.Add(user);
			}

			await context.SaveChangesAsync();
		}
	}
}