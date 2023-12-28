
using System.ComponentModel.DataAnnotations;
namespace SalesApi.src.Models;

public class Debt{

    [Key]
    [Required]
    public int Id { get; set; }

    [Required(ErrorMessage = "O valor da venda é obrigatório")]
    public int value { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    public DateTime? PaidAt { get; set; }

    [Required(ErrorMessage = "O identificador do cliente é obrigatório")]
    public int CustomerId { get; set; }

    public virtual Customer? Customer { get; set; }

    [Required]
   public int Active { get; set; }
    
}