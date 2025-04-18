using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController(DataContext context, ITokenService tokenService) : BaseApiController
{
		[HttpPost("register")]
		public async Task<ActionResult<UserDto>> Register([FromBody] RegisterDto registerDto)
		{
			if (await UserExists(registerDto.Username)) return BadRequest("Username is taken");

			using var hmac = new HMACSHA512();

			var user = new AppUser 
			{
				UserName = registerDto.Username.ToLower(),
				PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
				PasswordSalt = hmac.Key,
				KnownAs = string.Empty,
				Gender = string.Empty,
				City = string.Empty,
				Country = string.Empty
			};

			context.Users.Add(user);
			await context.SaveChangesAsync();

			return new UserDto {
				Username = user.UserName,
				Token = tokenService.CreateToken(user)
			};
		}

		

		private async Task<bool> UserExists(string username)
		{
			return await context.Users.AnyAsync(x => x.UserName.ToLower() == username.ToLower());
		}

		[HttpPost("login")]
		public async Task<ActionResult<UserDto>> Login([FromBody] LoginDto loginDto)
		{
			var user = await context.Users
				.FirstOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());

			if (user == null) return Unauthorized("Invalid username");

			using var hmac = new HMACSHA512(user.PasswordSalt);

			// should be same computed hash stored in the database by using the same salt
			var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
			for (int i = 0; i < computedHash.Length; i++)
			{
				if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid password");
			}

			return new UserDto {
				Username = user.UserName,
				Token = tokenService.CreateToken(user)
			};
		}
}