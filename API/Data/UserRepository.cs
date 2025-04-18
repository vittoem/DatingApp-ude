using API.DTOs;
using API.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatingApp.API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class UserRepository(DataContext context, IMapper mapper) : IUserRepository
{
	public async Task<IEnumerable<AppUser>> GetUsersAsync()
	{
		return await context.Users
		.Include(x => x.Photos)
		.ToListAsync();
	}

	public async Task<AppUser?> GetUserByIdAsync(int id)
	{
		return await context.Users.FindAsync(id);
	}

	public async Task<AppUser?> GetUserByUsernameAsync(string username)
	{
		// Include the Photos navigation property when fetching the user by username
		return await context.Users.Include(x => x.Photos).SingleOrDefaultAsync(x => x.UserName == username);
	}

	public async Task<bool> SaveAllAsync()
	{
		return await context.SaveChangesAsync() > 0;
	}

	public void Update(AppUser user)
	{
		// Explicitly set the state of the entity to Modified.
		context.Entry(user).State = EntityState.Modified;
	}

	public async Task<IEnumerable<MemberDto>> GetMembersAsync()
	{
		return await context.Users.ProjectTo<MemberDto>(mapper.ConfigurationProvider)
		.ToListAsync();
	}

	public async Task<MemberDto?> GetMemberAsync(string username)
	{
		return await context.Users.Where(x => x.UserName == username).ProjectTo<MemberDto>(mapper.ConfigurationProvider)
		.SingleOrDefaultAsync();
	}
}