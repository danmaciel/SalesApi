

using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using SalesApi.src.Data.Dtos;
using SalesApi.src.Services.Interface;

namespace SalesApi.src.Services;

public class UserService: IUserService{

    private IMapper _mapper;
    private UserManager<User> _userManager;

    private SignInManager<User> _signInManager;

    private TokenService _tokenService;

     public UserService(IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager, TokenService tokenService){
        _mapper = mapper;
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
    }

    public async Task RegisterUser(CreateUserDto dto)
    {
        User user = _mapper.Map<User>(dto);
        IdentityResult result = await _userManager.CreateAsync(user, dto.Password);

        if(!result.Succeeded){
           throw new ApplicationException("Ocorreum um erro na criação do usuário!");
        }
    }

    public async Task<string> Authenticate(UserLoginDto dto) {
        var resultado = await _signInManager.PasswordSignInAsync(dto.UserName, dto.Password, false, false);

        if (!resultado.Succeeded){
            throw new ApplicationException("Usuário não autenticado!");
        }

        var usuario = _signInManager
            .UserManager
            .Users
            .FirstOrDefault(user => user.NormalizedUserName == dto.UserName.ToUpper());

        if (usuario == null){
            throw new ApplicationException("Erro ao autenticar o usuário!");
        }

        var token = _tokenService.GenerateToken(usuario);

        return token;    
    }
}