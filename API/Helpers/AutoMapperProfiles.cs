using API.DTOs;
using API.Entities;
using API.Extensions;
using AutoMapper;
using DatingApp.API.DTOs;

namespace API.Helpers;

public class AutoMapperProfiles : Profile
{
	public AutoMapperProfiles()
	{
		// Add your mappings here
		CreateMap<AppUser, MemberDto>()
		 .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()))
		 .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.Photos.FirstOrDefault(x => x.IsMain)!.Url));
		CreateMap<Photo, PhotoDto>();
		CreateMap<MemberUpdateDto, AppUser>();
	}
}