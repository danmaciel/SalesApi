using System.ComponentModel.DataAnnotations;

public class CreateCustomerDto {

    [Required]
    public required string FullName { get; set; }

    [Required]
    public required string Cpf { get; set; }

    [Required]
    public DateOnly BornDate { get; set; }

    [DataType(DataType.EmailAddress)]
    public String? Email { get; set; }

}