namespace SalesApi.src.Data.Dtos;

public class ReadDebtDto{
     
    public int Id { get; set; }

    public int Value { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime PaidAt { get; set; }

    public int CustomerId { get; set; }

    public int Active { get; set; }

}