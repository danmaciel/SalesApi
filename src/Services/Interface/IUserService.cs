
using SalesApi.src.Data.Dtos;

namespace SalesApi.src.Services.Interface;

public interface IUserService{
    public Task<string> Authenticate(UserLoginDto dto);
    public Task RegisterUser(CreateUserDto dto);
}