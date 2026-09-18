#nullable enable
using AutoMapper;
using UserDirectory.Application.DTOs;
using UserDirectory.Domain.Entities;

namespace UserDirectory.Application.Mappings;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>().ReverseMap();
    }
}
