using System.ComponentModel.DataAnnotations;

using SalesApi.src.Data.Dtos;

public class ReadCustomerDto{

    public int Id { get; set; }

    public required string FullName { get; set; }

    public required string Cpf { get; set; }

    public DateOnly BornDate { get; set; }

    public String? Email { get; set; }

    public virtual ICollection<ReadDebtDto>? Debts { get; set; }

    public int Active { get; set; }
}