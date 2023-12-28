using System.ComponentModel.DataAnnotations;

public class UpdateCustomerDto{

    [Required]
    public required string FullName { get; set; }

    [Required]
    public required string Cpf { get; set; }

    [Required]
    public required DateOnly BornDate { get; set; }

    [DataType(DataType.EmailAddress)]
    public String? Email { get; set; }

    public int Active { get; set; }

}