using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Entities;
using API.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace API.Services;

public class TokenService(IConfiguration config) : ITokenService
{
	public string CreateToken(AppUser user)
	{
		var tokenKey = config["TokenKey"] ?? throw new Exception("TokenKey is not set in configuration.");
		if (tokenKey.Length < 64)
		{
			throw new Exception("TokenKey is too short. It should be at least 64 characters long.");
		}
		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));

		var claims = new List<Claim>
		{
			new(ClaimTypes.NameIdentifier, user.UserName),
		};

		var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

		var tokenDescriptor = new SecurityTokenDescriptor
		{
			Subject = new ClaimsIdentity(claims),
			Expires = DateTime.UtcNow.AddDays(7),
			SigningCredentials = signingCredentials
		};

		var tokenHandler = new JwtSecurityTokenHandler();
		var token = tokenHandler.CreateToken(tokenDescriptor);
		return tokenHandler.WriteToken(token);
	}
}