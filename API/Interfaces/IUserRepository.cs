using API.DTOs;
using API.Entities;

namespace DatingApp.API.Interfaces;
	
public interface IUserRepository
{
		Task<IEnumerable<AppUser>> GetUsersAsync();
		Task<AppUser?> GetUserByIdAsync(int id);
		Task<AppUser?> GetUserByUsernameAsync(string username);
		Task<bool> SaveAllAsync();
		void Update(AppUser user);

		Task<IEnumerable<MemberDto>> GetMembersAsync();
		Task<MemberDto?> GetMemberAsync(string username);
}

