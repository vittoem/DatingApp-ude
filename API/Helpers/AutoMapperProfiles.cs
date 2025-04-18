using API.DTOs;
using API.Entities;
using AutoMapper;
using DatingApp.API.Extensions;

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
	}
}