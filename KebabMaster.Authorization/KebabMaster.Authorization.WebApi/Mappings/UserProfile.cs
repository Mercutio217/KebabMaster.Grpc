using AutoMapper;
using KebabMaster.Authorization.Domain.Entities;
using KebabMaster.Authorization.Domain.Filter;
using KebabMaster.Authorization.DTOs;

namespace KebabMaster.Authorization.Mappings;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserRequest, UserFilter>();
        CreateMap<User, UserResponse>();
    }
}