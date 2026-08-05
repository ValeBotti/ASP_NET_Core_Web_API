using System.ComponentModel.DataAnnotations;

public class User
{
    [Key]
    public required int Uid { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public required string CardFullName { get; set; }

    public required string CardNumber { get; set; }

    public required int CardExpireMonth { get; set; }

    public required int CardExpireYear { get; set; }

    public required string CardCVV { get; set; }

    public int? LastOid { get; set; }

    public string? OrderStatus { get; set; }

}