

using System.ComponentModel.DataAnnotations;

namespace SalesApi.src.Data.Dtos;

public class UserLoginDto{

    [Required]
    public required string UserName { get; set; }

    [Required]
    public required string Password { get; set; }

}