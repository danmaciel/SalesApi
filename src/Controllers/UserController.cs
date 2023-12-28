
using Microsoft.AspNetCore.Mvc;
using SalesApi.src.Data.Dtos;
using SalesApi.src.Services.Interface;

namespace SalesApi.src.Controllers;

[ApiController]
[Route("[Controller]")]
public class UserController: ControllerBase{


    private IUserService _service;

    public UserController(IUserService service){
        _service = service;
    }


    [HttpPost]
    public async Task<IActionResult> UserAdd(CreateUserDto dto){
        await _service.RegisterUser(dto);
        return Ok("Usuário criado com sucesso!");
    }

    [HttpPost("authenticate")]
    public async Task<IActionResult> Authenticate(UserLoginDto dto){
        var token = await _service.Authenticate(dto);
        return Ok(token);
    }
}