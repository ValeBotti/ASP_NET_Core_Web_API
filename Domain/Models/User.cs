using System.ComponentModel.DataAnnotations;

public class User
{
    [Key]
    public int Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? CardFullName { get; set; }

    public string? CardNumber { get; set; }

    public int? CardExpireMonth { get; set; }

    public int? CardExpireYear { get; set; }

    public string? CardCVV { get; set; }

}