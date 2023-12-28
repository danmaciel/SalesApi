

using System.ComponentModel.DataAnnotations;

namespace SalesApi.src.Data.Dtos;

public class UpdateDebtDto{

    [Required(ErrorMessage = "O valor do débito é obrigatório")]
    public int value { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime PaidAt { get; set; }

    public int Active { get; set; }

}