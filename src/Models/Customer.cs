

using System.ComponentModel.DataAnnotations;
using SalesApi.src.Models;


public class Customer{

    [Key]
    [Required]
    public int Id { get; set; }

    [Required(ErrorMessage ="O nome é obrigatório")]
    public required string FullName { get; set; }

    [Required(ErrorMessage ="O cpf é obrigatório")]
    public required string Cpf { get; set; }

    [Required(ErrorMessage ="A data de nascimento é obrigatória")]
    public DateOnly BornDate { get; set; }

    public String? Email { get; set; }

    public virtual ICollection<Debt>? Debts { get; set; }

    [Required]
    public int Active { get; set; }
}