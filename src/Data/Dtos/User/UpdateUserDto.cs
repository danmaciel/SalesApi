
using System.ComponentModel.DataAnnotations;

namespace SalesApi.src.Data.Dtos;

public class UpdateUserDto{

    [Required]
    public required string UserName { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public required string Password { get; set; }

    [Required]
    [Compare("Password")]
    public required string RePassword { get; set; }

}