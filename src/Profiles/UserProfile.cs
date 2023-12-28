

using AutoMapper;
using SalesApi.src.Data.Dtos;

public class UserProfile: Profile{
    public UserProfile(){
        CreateMap<CreateUserDto, User>();
    }
}